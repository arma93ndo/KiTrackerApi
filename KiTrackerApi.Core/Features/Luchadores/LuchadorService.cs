using System.Xml.Schema;
using KiTrackerApi.Core.Extensions;
using KiTrackerApi.Core.Features.Luchadores.DTOs;
using KiTrackerApi.Core.Interfaces;
using KiTrackerApi.Core.Models;

namespace KiTrackerApi.Core.Features.Luchadores;

public class LuchadorService : ILuchadorService
{
    private readonly IUnitOfWork _uow;

    public LuchadorService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task ActualizarByIdAsync(int id, ActualizarLuchadorDto dto)
    {
        // 1. Obtengo la instancia específica (registro) de la BBDD que el usuario desea modificar.
        var luchador = await _uow.Luchadores.GetByIdAsync(id);

        if(luchador is null)
            // Imposible hacer la actualización.
            throw new KeyNotFoundException($"El luchador con Id '{id}' no existe en la BBDD. Actualización imposible.");

        // 2. Modifico la instancia obtenida directamente para que el Change Tracker la detecte.
        if(!string.IsNullOrWhiteSpace(dto.Nombre))
        {
            luchador.Nombre = dto.Nombre.SanitizarNombrePropio();
        }

        if(dto.EspecieId.HasValue)
        {
            var especieExistente = await _uow.Especies.GetByIdAsync(dto.EspecieId.Value);

            if(especieExistente is null)
                // El usuario intentó usar una especie inexistente en la BBDD.
                throw new KeyNotFoundException($"La especie con el id '{dto.EspecieId.Value}' no existe en la BBDD.");
            
            luchador.EspecieId = dto.EspecieId.Value;
        }

        if(!string.IsNullOrWhiteSpace(dto.FotoPerfilUrl))
        {
            luchador.FotoPerfilUrl = dto.FotoPerfilUrl.Trim();
        }

        // 3. Almaceno los cambios detectados por el Change Tracker en la BBDD.
        await _uow.SaveChangesAsync();
    }

    public async Task ActualizarByNombreAsync(string nombre, ActualizarLuchadorDto dto)
    {
        // 1. Me aseguro de que el usuario haya mandado un nombre qué buscar.
        if(string.IsNullOrWhiteSpace(nombre))
            // Imposible realizar la actualización.
            throw new ArgumentException($"El nombre proporcionado es inválido. Actualización imposible.");

        // 2. Limpio la cadena recibida para la búsqueda.
        var limpio = nombre.SanitizarNombrePropio();

        // 3. Obtengo la instancia de Luchador específica de la BBDD.
        var luchador = await _uow.Luchadores.GetByNombreAsync(limpio);

        if(luchador is null)
            throw new KeyNotFoundException($"El luchador con el Id especificado no existe en la BBDD.");

        // 4. Actualizo las propiedades solicitadas en la entidad hallada para que sea detectada por el Change Tracker.
        if(!string.IsNullOrWhiteSpace(dto.Nombre))
        {
            luchador.Nombre = dto.Nombre.SanitizarNombrePropio();
        }

        if(dto.EspecieId.HasValue)
        {
            var especieExistente = await _uow.Especies.GetByIdAsync(dto.EspecieId.Value);

            if(especieExistente is null)
                // El usuario intentó usar una especie inexistente en la BBDD.
                throw new KeyNotFoundException($"La especie con el Id especificado no existe en la BBDD.");
            luchador.EspecieId = dto.EspecieId.Value;
        }

        if(!string.IsNullOrWhiteSpace(dto.FotoPerfilUrl))
        {
            luchador.FotoPerfilUrl = dto.FotoPerfilUrl.Trim();
        }  

        // 5. Almaceno los cambios detectados por el Change Tracker en la BBDD.
        await _uow.SaveChangesAsync();
    }

    public async Task<LuchadorRespuestaDto> CrearLuchadorAsync(CrearLuchadorDto dto)
    {
        // 1. Garantizo que el ID de especie que envió el usuario exista en la BBDD.
        var especieExistente = await _uow.Especies.GetByIdAsync(dto.EspecieId);
        if(especieExistente is null)
        {
            throw new KeyNotFoundException($"La especie con Id {dto.EspecieId} no existe en la BBDD. Creación del Luchador imposible.");
        }

        // 2. Comienzo a poblar al nuevo luchador que se creará en la BBDD.
        var nuevoLuchador = new Luchador
        {
            Nombre = dto.Nombre.SanitizarNombrePropio(),
            EspecieId = dto.EspecieId,
            FotoPerfilUrl = string.IsNullOrWhiteSpace(dto.FotoPerfilUrl) ? null : dto.FotoPerfilUrl.Trim(),
            FechaRegistro = DateTime.UtcNow
        };

        // 3. Almaceno en la BBDD la nueva instancia de "Luchador" con los datos mandados por el usuario.
        await _uow.Luchadores.AddAsync(nuevoLuchador);
        await _uow.SaveChangesAsync();

        // 4. Comienzo a poblar la instancia de "LuchadorRespuestaDto" que le enviaré de vuelta al cliente.
        var respuesta = new LuchadorRespuestaDto
        {
            Id = nuevoLuchador.Id,
            Nombre = nuevoLuchador.Nombre,
            EspecieId = nuevoLuchador.EspecieId,
            NombreEspecie = nuevoLuchador.Especie?.Descripcion ?? string.Empty
        };

        return respuesta;
    }

    public async Task EliminarByIdAsync(int id)
    {
        // 1. Obtengo la instancia específica de la BBDD que el usuario desea eliminar.
        var luchador = await _uow.Luchadores.GetByIdAsync(id);

        if(luchador is null)
            throw new KeyNotFoundException($"El luchador con el Id '{id}' no existe en la BBDD. Eliminación imposible.");

        // 2. Realizo la operación de borrado que será detectada por el Change Tracker.
        _uow.Luchadores.Remove(luchador);

        // 3. Almaceno los cambios realizados en la BBDD.
        await _uow.SaveChangesAsync();
    }

    public async Task EliminarByNombreAsync(string nombre)
    {
        // 1. Me aseguro de haber recibido un nombre por parte del usuario.
        if(string.IsNullOrWhiteSpace(nombre))
            // Imposible realizar la eliminación.
            throw new ArgumentException($"El nombre proporcionado es inválido. Eliminación imposible.");

        // 2. Limpio la cadena recibida para la búsqueda.
        var limpio = nombre.SanitizarNombrePropio();
        
        // 3. Obtengo la instancia específica por parte de la BBDD.
        var luchador = await _uow.Luchadores.GetByNombreAsync(limpio);

        if(luchador is null)
            throw new KeyNotFoundException($"El luchador proporcionado no existe en la BBDD.");

        // 4. Realizo la operación de borrado para que sea detectada por el Change Tracker.
        _uow.Luchadores.Remove(luchador);

        // 5. Guardo los cambios realizados en la BBDD.
        await _uow.SaveChangesAsync();
    }

    public async Task<IEnumerable<LuchadorRespuestaDto>> ObtenerByEspecieIdAsync(int especieId)
    {
        // 1. Garantizo la existencia de la especie enviada por el usuario.
        var especieExistente = await _uow.Especies.GetByIdAsync(especieId);

        if(especieExistente is null)
            throw new KeyNotFoundException($"La especie con Id {especieId} no existe en la BBDD.");
        
        // 2. Obtengo las instancias de "Luchador" que coincidan con el filtro.
        var luchadores = await _uow.Luchadores.GetByEspecieIdAsync(especieId);

        // 3. Mapeo los luchadores obtenidos al formato esperado por el cliente (LuchadorRespuestaDto)
        return (luchadores ?? Enumerable.Empty<Luchador>()).Select(l => new LuchadorRespuestaDto
        {
            Id = l.Id,
            Nombre = l.Nombre,
            EspecieId = l.EspecieId,
            NombreEspecie = l.Especie?.Descripcion ?? string.Empty
        });
    }

    public async Task<LuchadorRespuestaDto?> ObtenerByIdAsync(int id)
    {
        // 1. Obtengo la instancia especificada por parte de la BBDD.
        var luchador = await _uow.Luchadores.GetByIdAsync(id);

        if(luchador is null)
            // El registro no existe en la BBDD.
            return null;

        // 2. Mapeo las propiedades de la instancia obtenida a su formato esperado (LuchadorRespuestaDto).
        return new LuchadorRespuestaDto
        {
            Id = luchador.Id,
            Nombre = luchador.Nombre,
            EspecieId = luchador.EspecieId,
            NombreEspecie = luchador.Especie?.Descripcion ?? string.Empty
        };
    }

    public async Task<LuchadorRespuestaDto?> ObtenerByNombreAsync(string nombre)
    {
        // 1. Me aseguro de que el usuario haya enviado un nombre.
        if(string.IsNullOrWhiteSpace(nombre))
            // Imposible realizar la búsqueda.
            return null;
        // 2. Limpio la cadena que usaré para la búsqueda.
        var limpio = nombre.SanitizarNombrePropio();

        // 3. Busco la instancia solicitada usando la UoW.
        var luchador = await _uow.Luchadores.GetByNombreAsync(nombre);

        if(luchador is null) return null;

        // 4. Mapeo la respuesta obtenida a su formato esperado (LuchadorRespuestaDto).
        return new LuchadorRespuestaDto
        {
            Id = luchador.Id,
            Nombre = luchador.Nombre,
            EspecieId = luchador.EspecieId,
            NombreEspecie = luchador.Especie?.Descripcion ?? string.Empty
        };
    }

    public async Task<IEnumerable<LuchadorRespuestaDto>> ObtenerTodosAsync()
    {
        var luchadores = await _uow.Luchadores.GetAllConEspecieAsync();

        return (luchadores ?? Enumerable.Empty<Luchador>()).Select(l => new LuchadorRespuestaDto
        {
            Id = l.Id,
            Nombre = l.Nombre,
            EspecieId = l.EspecieId,
            NombreEspecie = l.Especie?.Descripcion ?? string.Empty
        });
    }
}