using KiTrackerApi.Core.Models;
using KiTrackerApi.Core.Features.Dispositivos.DTOs;
using KiTrackerApi.Core.Interfaces;
using KiTrackerApi.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace KiTrackerApi.Core.Features.Dispositivos;

public class DispositivoService : IDispositivoService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<DispositivoService> _logger;

    public DispositivoService(IUnitOfWork unitOfWork, ILogger<DispositivoService> logger)
    {
        _uow = unitOfWork;
        _logger = logger;

    }

    public async Task<DispositivoRespuestaDto> ActualizarByIdAsync(int id, ActualizarDispositivoDto dto)
    {
        // 1. Verifico que el dispositivo solicitado exista.
        var dispositivo = await _uow.Dispositivos.GetByIdConDetallesAsync(id);
        if(dispositivo is null)
            // Actualización imposible.
            throw new KeyNotFoundException($"El dispositivo con el id '{id}' no existe en la BBDD. Actualización imposible.");

        // 2. Comienzo a actualizar las propiedades solicitadas, modificándolas directamente sobre el objeto en memoria.
        if(!string.IsNullOrWhiteSpace(dto.Tipo))
        {
            dispositivo.Tipo = dto.Tipo.Trim();
        }

        if(!string.IsNullOrWhiteSpace(dto.ModeloHardware))
        {
            dispositivo.ModeloHardware = dto.ModeloHardware.Trim();
        }

        if(dto.ColorId.HasValue)
        {
            var color = await _uow.Colores.GetByIdAsync(dto.ColorId.Value);
            if(color is null)
                // Color inexistente.
                throw new KeyNotFoundException($"El color proporcionado '{dto.ColorId}' no existe en la BBDD.");
            
            dispositivo.Color = color;
        }

        // 3. Alamaceno los datos realizados a la instancia en la BBDD.
        await _uow.SaveChangesAsync();
        _logger.LogInformation("El Dispositivo con Id {dispositivoId} se actualizó correctamente.", dispositivo.Id);

        // 4. Comienzo a mapear los datos actualizados en el formato esperado por el cliente (DispositivoRespuestaDto).
        var respuesta = new DispositivoRespuestaDto
        {
            Id = dispositivo.Id,
            Fingerprint = dispositivo.Fingerprint,
            Tipo = dispositivo.Tipo,
            FechaUltimoUso = dispositivo.FechaUltimoUso,
            ColorId = dispositivo.ColorId,
            NombreColor = dispositivo.Color?.Descripcion ?? string.Empty
        };

        return respuesta;
    }

    public async Task<DispositivoRespuestaDto> CrearDispositivoAsync(CrearDispositivoDto dto)
    {
        // 1. Verifico la unicidad del Fingerprint proporcionado.
        var fingerprintLimpio = dto.Fingerprint.Trim();
        var dispositivoExistente = await _uow.Dispositivos.GetByFingerprintConDetallesAsync(fingerprintLimpio);        

        if(dispositivoExistente is not null)
            // Conflicto con un fingerprint ya existente en la BBDD.
            throw new InvalidOperationException($"Ya existe un dispositivo registrado con el fingerprint proporcionado. Creación imposible.");

        // 2. Verifico que las dependencias existan.
        var color = await _uow.Colores.GetByIdAsync(dto.ColorId);

        if(color is null)
        // Color inexistente.
            throw new KeyNotFoundException($"El color con Id '{dto.ColorId}' proporcionado no existe en la BBDD. Creación imposible.");

        // 3. Comienzo a poblar una nueva instancia de "Dispositivo" con los datos del DTO.
        var nuevoDispositivo = new Dispositivo
        {
            Fingerprint = fingerprintLimpio,
            Tipo = dto.Tipo.SanitizarNombrePropio(),
            ModeloHardware = dto.ModeloHardware ?? string.Empty,
            ColorId = color.Id,
            FechaUltimoUso = DateTime.UtcNow
        };

        // 4. Almaceno la instancia recién creada en la BBDD.
        await _uow.Dispositivos.AddAsync(nuevoDispositivo);
        await _uow.SaveChangesAsync();
        _logger.LogInformation("El Dispositivo con Id {dispositivoId} se creó correctamente.", nuevoDispositivo.Id);

        // 5. Pueblo la respuesta que enviaré al cliente (DispositivoRespuestaDto).
        var respuesta = new DispositivoRespuestaDto
        {
            Id = nuevoDispositivo.Id,
            Fingerprint = nuevoDispositivo.Fingerprint,
            Tipo = nuevoDispositivo.Tipo,
            FechaUltimoUso = nuevoDispositivo.FechaUltimoUso,
            ColorId = color.Id,
            NombreColor = color.Descripcion
        };

        return respuesta;
    }

    public async Task EliminarByIdAsync(int id)
    {
        // 1. Obtengo la instancia de dispositivo específica desde la BBDD.
        var dispositivo = await _uow.Dispositivos.GetByIdAsync(id);
        if(dispositivo is null)
            // Dispositivo inexistente.
            throw new KeyNotFoundException($"El dispositivo con el Id '{id}' proporcionado no existe en la BBDD. Eliminación imposible.");
        
        // 2. Lo elimino de la colección de dispositivos para que lo detecte el Change Tracker.
        _uow.Dispositivos.Remove(dispositivo);

        // 3. Almaceno los cambios realizados.
        await _uow.SaveChangesAsync();
        _logger.LogInformation("El Dispositivo con Id {dispositivoId} se eliminó correctamente.", id);
    }

    public async Task<IEnumerable<DispositivoRespuestaDto>> ObtenerByColorIdAsync(int colorId)
    {
        // 1. Verifico la existencia de las dependencias.
        var colorExistente = await _uow.Colores.GetByIdAsync(colorId);

        if(colorExistente is null)
            // Color inexistente.
            throw new KeyNotFoundException($"El color con Id '{colorId}' proporcionado no existe en la BBDD.");
        
        // 2. Obtengo todas las instancis solicitadas desde la BBDD.
        var dispositivos = await _uow.Dispositivos.GetByColorIdConDetallesAsync(colorId);

        return (dispositivos ?? Enumerable.Empty<Dispositivo>()).Select(d => new DispositivoRespuestaDto
        {
            Id = d.Id,
            Fingerprint = d.Fingerprint,
            Tipo = d.Tipo,
            FechaUltimoUso = d.FechaUltimoUso,
            ColorId = colorExistente.Id,
            NombreColor = colorExistente.Descripcion
        });
    }

    public async Task<DispositivoRespuestaDto> ObtenerByFingerprintAsync(string fingerprint)
    {
        // 1. Valido que el dispositivo solicitado exista.
        var dispositivo = await _uow.Dispositivos.GetByFingerprintConDetallesAsync(fingerprint);
        if(dispositivo is null)
            // Dispositivo inexistente.
            throw new KeyNotFoundException($"El dipositivo con el fingerprint '{fingerprint}' proporcionado no existe en la BBDD.");

        // 2. Elaboro la respuesta al cliente con el formato que espera (DispositivoRespuestaDto).
        var respuesta = new DispositivoRespuestaDto
        {
            Id = dispositivo.Id,
            Fingerprint = dispositivo.Fingerprint,
            Tipo = dispositivo.Tipo,
            FechaUltimoUso = dispositivo.FechaUltimoUso,
            ColorId = dispositivo.ColorId,
            NombreColor = dispositivo.Color?.Descripcion ?? string.Empty
        };

        return respuesta;
    }

    public async Task<DispositivoRespuestaDto> ObtenerByIdAsync(int id)
    {
        // 1. Obtengo la instancia de dipositivo solicitada.
        var dispositivo = await _uow.Dispositivos.GetByIdConDetallesAsync(id);

        if(dispositivo is null)
            // Dispositivo inexistente.
            throw new KeyNotFoundException($"El dispositivo con Id '{id}' no existe en la BBDD.");

        // 2. Pueblo una nueva instancia de DispositivoRespuestaDto que devolveré al cliente.
        var respuesta = new DispositivoRespuestaDto
        {  
           Id = dispositivo.Id,
           Fingerprint = dispositivo.Fingerprint,
           Tipo = dispositivo.Tipo,
           FechaUltimoUso = dispositivo.FechaUltimoUso,
           ColorId = dispositivo.ColorId,
           NombreColor = dispositivo.Color?.Descripcion ?? string.Empty 
        };

        return respuesta;
    }

    public async Task<IEnumerable<DispositivoRespuestaDto>> ObtenerTodosConDetallesAsync()
    {
        // 1. Obtengo todas las instancias existentes usando la UnitOfWork.
        var dispositivos = await _uow.Dispositivos.GetAllConDetallesAsync();

        // 2. Regreso los datos usando el formato esperado (DispositivoRespuestaDto).
        return (dispositivos ?? Enumerable.Empty<Dispositivo>()).Select(d => new DispositivoRespuestaDto
        {
            Id = d.Id,
            Fingerprint = d.Fingerprint,
            Tipo = d.Tipo,
            FechaUltimoUso = d.FechaUltimoUso,
            ColorId = d.ColorId,
            NombreColor = d.Color?.Descripcion ?? string.Empty
        });
    }
}