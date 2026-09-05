using System.Data.Common;
using KiTrackerApi.Core.Extensions;
using KiTrackerApi.Core.Features.Lecturas.DTOs;
using KiTrackerApi.Core.Interfaces;
using KiTrackerApi.Core.Models;

namespace KiTrackerApi.Core.Features.Lecturas;

public class LecturaService : ILecturaService
{
    private readonly IUnitOfWork _uow;

    public LecturaService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<LecturaRespuestaDto> CrearLecturaAsync(CrearLecturaDto dto)
    {
        // 1. Garantizo que las relaciones con "Luchador" y "Dispositivo" sean válidas.
        var luchadorExistente = await _uow.Luchadores.GetByIdConEspecieAsync(dto.LuchadorId);

        if(luchadorExistente is null)
            // TODO: Corregir esta excepción con el nuevo formato Problem Details.
            throw new KeyNotFoundException($"El luchador con el Id {dto.LuchadorId} no existe en la BBDD. Creación de Lectura imposible.");
        
        if(string.IsNullOrWhiteSpace(dto.FingerprintDispositivo))
            // Imposible hacer la creación.
            // TODO: Corregir esta excepción con el nuevo formato Problem Details.
            throw new ArgumentException($"El fingerprint de dispositivo enviado no es válido. Creación del Lectura imposible.");

        var fingerprintLimpio = dto.FingerprintDispositivo.Trim();
        var dispositivoExistente = await _uow.Dispositivos.GetByFingerprintConDetallesAsync(fingerprintLimpio);
        if(dispositivoExistente is null)
            // TODO: Corregir esta excepción con el nuevo formato Problem Details.
            throw new KeyNotFoundException($"El dispositivo con el Id {dto.FingerprintDispositivo} no existe en la BBDD. Creación de Lectura imposible.");
        
        // 2. Como el dispositivo obtenido está creando la nueva lectura, actualizo su fecha de uso para que el Change Tracker lo detecte.
        var horaActual = DateTime.UtcNow;
        dispositivoExistente.FechaUltimoUso = horaActual;

        // 3. Comienzo a poblar las propiedades de la nueva instancia de Lectura.
        var nuevaLectura = new Lectura
        {
            LuchadorId = dto.LuchadorId,
            DispositivoId = dispositivoExistente.Id,
            NivelKi = dto.NivelKi,
            RutaFotografia = string.IsNullOrWhiteSpace(dto.RutaFotografia) ? null : dto.RutaFotografia.Trim(),
            FechaLectura = horaActual
        };

        // 4. Almaceno la nueva instancia de Lectura en la BBDD vía la UoW.
        await _uow.Lecturas.AddAsync(nuevaLectura);
        await _uow.SaveChangesAsync();

        // 5. Comienzo a poblar la instancia de "LecturaRespuestaDto" que le enviaré de vuelta al cliente.
        var respuesta = new LecturaRespuestaDto
        {
            Id = nuevaLectura.Id,
            LuchadorId = nuevaLectura.LuchadorId,
            DispositivoId = nuevaLectura.DispositivoId,
            NivelKi = nuevaLectura.NivelKi,
            FechaLectura = nuevaLectura.FechaLectura,
            NombreLuchador = luchadorExistente.Nombre,
            NombreEspecie = luchadorExistente.Especie?.Descripcion
        };

        return respuesta;
    }

    public async Task EliminarByIdAsync(int id)
    {
        // 1. Obtengo la lectura específica desde la BBDD.
        var lectura = await _uow.Lecturas.GetByIdAsync(id);

        if(lectura is null)
            // Eliminación imposible.
            throw new KeyNotFoundException($"La lecutra con el Id '{id}' no existe en la BBDD. Eliminación imposible.");
            
        // 2. Realizo la operación de borrado para que el Change Tracker la detecte.
        _uow.Lecturas.Remove(lectura);

        // 3. Almaceno los cambios realizados en la BBDD.
        await _uow.SaveChangesAsync();
    }

    public async Task<IEnumerable<LecturaRespuestaDto>> ObtenerByEspecieIdAsync(int especieId)
    {
        // 1. Corroboro que las dependencias existan.
        var especieExistente = await _uow.Especies.GetByIdAsync(especieId);

        if(especieExistente is null)
            // Especie inexistente.
            throw new KeyNotFoundException($"El Id de especie proporcionado '{especieId}' no existe en la BBDD.");
        
        // 2. Obtengo las instancias solicitadas desde la BBDD.
        var lecturas = await _uow.Lecturas.GetByEspecieIdAsync(especieId);

        // 3. Mapeo los datos al formato "LecturaRespuestaDto" esperada por el cliente.
        return (lecturas ?? Enumerable.Empty<Lectura>()).Select(l => new LecturaRespuestaDto
        {
            Id = l.Id,
            LuchadorId = l.LuchadorId,
            DispositivoId = l.DispositivoId,
            NivelKi = l.NivelKi,
            FechaLectura = l.FechaLectura,
            NombreLuchador = l.Luchador?.Nombre ?? string.Empty,
            NombreEspecie = l.Luchador?.Especie?.Descripcion ?? especieExistente.Descripcion
        });
    }

    public async Task<LecturaRespuestaDto?> ObtenerByIdAsync(int id)
    {
        // 1. Obtengo la instancia especifica desde la BBDD.
        var lectura = await _uow.Lecturas.GetByIdConDetallesAsync (id);

        if(lectura is null)
            // Lectura inexistente.
            return null;

        // 2. Pueblo el objeto "LecturaRespuestaDto" que devolveré al cliente.
        var respuesta = new LecturaRespuestaDto
        {
            Id = lectura.Id,
            LuchadorId = lectura.LuchadorId,
            DispositivoId = lectura.DispositivoId,
            NivelKi = lectura.NivelKi,
            FechaLectura = lectura.FechaLectura,
            NombreLuchador = lectura.Luchador?.Nombre ?? string.Empty,
            NombreEspecie = lectura.Luchador?.Especie?.Descripcion
        };

        return respuesta;
    }

    public async Task<IEnumerable<LecturaRespuestaDto>> ObtenerByLuchadorIdAsync(int luchadorId)
    {
        // 1. Corroboro que el luchador solicitado exista.
        var luchadorExistente = await _uow.Luchadores.GetByIdAsync(luchadorId);

        if(luchadorExistente is null)
            // Solicitan un luchador inexistente en la BBDD.
            throw new KeyNotFoundException($"El luchador con el Id '{luchadorId}' no existe en la BBDD.");

        // 2. Obtenemos las lecturas en BBDD del luchador especificado.
        var lecturas = await _uow.Lecturas.GetByLuchadorIdConDetallesAsync(luchadorId);

        return (lecturas ?? Enumerable.Empty<Lectura>()).Select(l => new LecturaRespuestaDto
        {
            Id = l.Id,
            LuchadorId = l.LuchadorId,
            DispositivoId = l.DispositivoId,
            NivelKi = l.NivelKi,
            FechaLectura = l.FechaLectura,
            NombreLuchador = l.Luchador?.Nombre ?? string.Empty,
            NombreEspecie = l.Luchador?.Especie?.Descripcion
        });
    }

    public async Task<IEnumerable<LecturaRespuestaDto>> ObtenerByNombreLuchadorAsync(string nombreLuchador)
    {
        // 1. Verifico recibir datos válidos por parte del cliente.
        if(string.IsNullOrWhiteSpace(nombreLuchador))
            throw new ArgumentException($"El nombre de luchador proporcionado no puede estar vacío.", nameof(nombreLuchador));

        // 2. Verifico la existencia de las dependencias en la BBDD.
        var limpio = nombreLuchador.SanitizarNombrePropio();
        var luchadorExistente = await _uow.Luchadores.GetByNombreAsync(limpio);

        if(luchadorExistente is null)
            // Dependencias inexistentes.
            throw new KeyNotFoundException($"El luchador con el nombre '{limpio}' no existe en la BBDD.");
        
        // 3. Obtengo las lecturas relacionadas con el luchador proporcionado.
        var lecturas = await _uow.Lecturas.GetByLuchadorIdConDetallesAsync(luchadorExistente.Id);

        // 4. Mapeo los datos recuperados al formato "LecturaRespuestaDto" esperado por el cliente.
        return (lecturas ?? Enumerable.Empty<Lectura>()).Select(l => new LecturaRespuestaDto
        {
            Id = l.Id,
            LuchadorId = l.LuchadorId,
            DispositivoId = l.DispositivoId,
            NivelKi = l.NivelKi,
            FechaLectura = l.FechaLectura,
            NombreLuchador = l.Luchador?.Nombre ?? string.Empty,
            NombreEspecie = l.Luchador?.Especie?.Descripcion
        });
    }

    public async Task<IEnumerable<LecturaRespuestaDto>> ObtenerByRangoDeKiAsync(long minimo, long maximo)
    {
        // 1. Validaciones defensivas iniciales de los argumentos de entrada.
        if(minimo < 0)
            throw new ArgumentOutOfRangeException(nameof(minimo), $"El valor mínimo de un rango de ki no puede ser negativo.");

        if(maximo < 0)
            throw new ArgumentOutOfRangeException(nameof(maximo), $"El valor máximo de un rango de ki no puede ser negativo.");

        if(maximo < minimo)
            throw new ArgumentException($"El rango de ki es inválido: El valor mínimo {minimo} es mayor que el valor máximo {maximo}.");

        // 2. Obtengo las instancias de Lectura que concuerden con estas restricciones.
        var lecturas = await _uow.Lecturas.GetLecturasKiInsideRango(minimo, maximo);

        return (lecturas ?? Enumerable.Empty<Lectura>()).Select(l => new LecturaRespuestaDto
        {
            Id = l.Id,
            LuchadorId = l.LuchadorId,
            DispositivoId = l.DispositivoId,
            NivelKi = l.NivelKi,
            FechaLectura = l.FechaLectura,
            NombreLuchador = l.Luchador?.Nombre ?? string.Empty,
            NombreEspecie = l.Luchador?.Especie?.Descripcion
        });
    }

    public async Task<IEnumerable<LecturaRespuestaDto>> ObtenerMaximosKisAsync(int top)
    {
        const int limiteMaximoTop = 250;

        // 1. Validaciones básicas a los parámetros de entrada.
        if(top <= 0 || top > limiteMaximoTop)
            throw new ArgumentOutOfRangeException(nameof(top), $"El valor de top proporcionado debe estar entre 1 y {limiteMaximoTop}.");
        
        // 2. Obtengo las "top" instancias de mayor ki de la tabla Lecturas.
        var lecturasTop = await _uow.Lecturas.GetTopKiLecturasAsync(top);

        // 3. Mapeo los datos obtenidos al tipo de respuesta (LecturaRespuestaDto) esperado por el cliente.
        return (lecturasTop ?? Enumerable.Empty<Lectura>()).Select(l => new LecturaRespuestaDto
        {
            Id = l.Id,
            LuchadorId = l.LuchadorId,
            DispositivoId = l.DispositivoId,
            NivelKi = l.NivelKi,
            FechaLectura = l.FechaLectura,
            NombreLuchador = l.Luchador?.Nombre ?? string.Empty,
            NombreEspecie = l.Luchador?.Especie?.Descripcion
        });
    }

    public async Task<IEnumerable<LecturaRespuestaDto>> ObtenerTodasAsync()
    {
        var lecturas = await _uow.Lecturas.GetTodasConDetallesAsync();

        return (lecturas ?? Enumerable.Empty<Lectura>()).Select(l => new LecturaRespuestaDto
        {
            Id = l.Id,
            LuchadorId = l.LuchadorId,
            DispositivoId = l.DispositivoId,
            NivelKi = l.NivelKi,
            FechaLectura = l.FechaLectura,
            NombreLuchador = l.Luchador?.Nombre ?? string.Empty,
            NombreEspecie = l.Luchador?.Especie?.Descripcion
        });
    }
}