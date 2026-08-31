using System.Globalization;
using KiTrackerApi.Core.Features.Especies.DTOs;
using KiTrackerApi.Core.Interfaces;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Core.Extensions;

namespace KiTrackerApi.Core.Features.Especies;

public class EspecieService : IEspecieService
{
    // Constantes.

    // Variables de instancia.
    private readonly IUnitOfWork _uow;

    public EspecieService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<bool> ActualizarEspecieByIdAsync(int id, ActualizarEspecieDto dto)
    {
        // 1. Obtener una entidad (modelo) rastreada por EF Core mediante el repositorio genérico.
        var especie = await _uow.Especies.GetByIdAsync(id);

        if(especie is null) return false;

        // 2. Modifico las propiedades directamente en la entidad almacenada en memoria para que EF Core las detecte.
        if(!string.IsNullOrWhiteSpace(dto.Descripcion))
        {
            especie.Descripcion = dto.Descripcion.SanitizarNombrePropio();
        }
        
        if(dto.Multiplicador.HasValue) // Modifico el valor del multiplicador sólo si el cliente mandó su valor explícito en el DTO.
        {
            especie.Multiplicador = dto.Multiplicador.Value;
        }

        // 3. El método .SaveChangesAsync() detecta los cambios del Change Tracker y genera la consulta UPDATE de SQL.
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ActualizarEspecieByNombreAsync(string nombre, ActualizarEspecieDto dto)
    {
        if(string.IsNullOrWhiteSpace(nombre))
            // Imposible hacer la actualización.
            return false;

        // 1. Limpio la cadena recibida para la búsqueda.
        string limpio = nombre.SanitizarNombrePropio();

        // 2. Obtengo la instancia de la BBDD buscada por el usuario, ignorando mayúsculas y minúsculas.
        var especies = await _uow.Especies.GetAllAsync();

        var especie = (especies ?? Enumerable.Empty<Especie>())
            .FirstOrDefault(e => string.Equals(e.Descripcion.SanitizarNombrePropio(), limpio, StringComparison.OrdinalIgnoreCase));

        if(especie is null) return false;

        // 3. Modifico las propiedades solicitadas directamente en la entidad almacenada en memoria para que EF Core detecte los cambios.
        if(!string.IsNullOrWhiteSpace(dto.Descripcion))
        {
            especie.Descripcion = dto.Descripcion.SanitizarNombrePropio();
        }

        if(dto.Multiplicador.HasValue)
        {
            especie.Multiplicador = dto.Multiplicador.Value;
        }

        // 4. El método .SaveChangesAsync() detecta los cambios en el Change Tracker y genera la consulta UPDATE de SQL.
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<EspecieRespuestaDto> CrearEspecieAsync(CrearEspecieDto dto)
    {
        const double MULTIPLICADOR_POR_DEFECTO = 1.0; // Valor por omisión del multiplicador

        // Comienzo a poblar una nueva instancia del modelo "Especie".
        var nuevaEspecie = new Especie
        {
            Descripcion = dto.Descripcion,
            Multiplicador = dto.Multiplicador ?? MULTIPLICADOR_POR_DEFECTO
        };

        // Almaceno en la BBDD la nueva instancia de "Especie" mandada por el usuario.
        await _uow.Especies.AddAsync(nuevaEspecie);
        await _uow.SaveChangesAsync();

        // Pueblo una nueva instancia de "EspecieRespuestaDto" que es el objeto que se devolverá al cliente.
        var respuesta = new EspecieRespuestaDto
        {
            Id = nuevaEspecie.Id, // El Id generado automáticamente por la BBDD.
            Descripcion = nuevaEspecie.Descripcion
        };

        return respuesta;
    }

    public async Task<bool> EliminarEspecieByIdAsync(int id)
    {
        // 1. Obtengo la entidad (modelo) que el usuario desea eliminar.
        var especie = await _uow.Especies.GetByIdAsync(id);

        if(especie is null) return false;

        // 2. Elimino la especie particular utilizando la UoW para que el Change Tracker lo detecte.
        _uow.Especies.Remove(especie);

        // 3. Almaceno los cambios realizados a la BBDD.
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminarEspecieByNombreAsync(string nombre)
    {
        // 1. Garantizo que el usuario haya enviado alguna información.
        if(string.IsNullOrWhiteSpace(nombre)) return false;

        // 2. Limpio la cadena recibida para la búsqueda.
        string limpio = nombre.SanitizarNombrePropio();

        // 3. Obtengo las instancias diponibles en la BBDD ignorando minúsculas y mayúsculas para su posterior filtrado.
        var especies = await _uow.Especies.GetAllAsync();
        var especie = (especies ?? Enumerable.Empty<Especie>())
            .FirstOrDefault(e => string.Equals(e.Descripcion.SanitizarNombrePropio(), limpio, StringComparison.OrdinalIgnoreCase));

        if(especie is null)
            // Especie no encontrada.
            return false;

        // 4. Elimino la instancia obtenido usando la Unit of Work.
        _uow.Especies.Remove(especie);
        // 5. Almaceno los cambios realizados a la BBDD.
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<EspecieRespuestaDto>> ObtenerTodasAsync()
    {
        // 1. Obtener todas las entidades (registros) de la BBDD.
        var especies = await _uow.Especies.GetAllAsync();

        // 2. Mapear la colección de entidades tipo "Especie" a "EspecieRespuestaDto".
        return (especies ?? Enumerable.Empty<Especie>()).Select(e => new EspecieRespuestaDto
        {
            Id = e.Id,
            Descripcion = e.Descripcion
        });
    }
}