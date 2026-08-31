using KiTrackerApi.Core.Interfaces;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Core.Features.Colores.DTOs;
using KiTrackerApi.Core.Extensions;
using System.Data.Common;

namespace KiTrackerApi.Core.Features.Colores;

public class ColorService : IColorService
{
    private readonly IUnitOfWork _uow;

    public ColorService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<bool> ActualizarColorByIdAsync(int id, ActualizarColorDto dto)
    {
        // 1. Obtengo la instancia específica por medio del repositorio genérico.
        var color = await _uow.Colores.GetByIdAsync(id);

        if(color is null)
            // Imposible realizar la actualización.
            return false;

        // 2. Modifico las propiedades directamente en la entidad almacenada en memroai para que EF Core detecte los cambios.
        if(!string.IsNullOrWhiteSpace(dto.Descripcion))
        {
            color.Descripcion = dto.Descripcion.SanitizarNombrePropio();
        }

        if(!string.IsNullOrWhiteSpace(dto.CodigoHex))
        {
            color.CodigoHex = dto.CodigoHex.Trim().ToUpperInvariant();
        }

        // 3. Almaceno los cambios del Change Tracker y genera la consulta UPDATE de SQL.
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ActualizarColorByNombreAsync(string nombre, ActualizarColorDto dto)
    {
        if(string.IsNullOrWhiteSpace(nombre))
            // Imposible hacer la actualización.
            return false;

        // 1. Sanitizo la cadena del nombre antes de usarla para buscar.
        var limpio = nombre.SanitizarNombrePropio();
    
        // 2. Obtengo la instancia de la BBDD buscada por el usuario, ignorándo mayúsculas y minúsculas.
        var colores = await _uow.Colores.GetAllAsync();
        var color = (colores ?? Enumerable.Empty<Color>())
            .FirstOrDefault(c => string.Equals(c.Descripcion.SanitizarNombrePropio(), limpio, StringComparison.OrdinalIgnoreCase));

        if(color is null) return false;

        // 3. Modifico las propiedades solicitadas directamente en la entidad almacenada en memoria para que EF Core detecte los cambios.
        if(!string.IsNullOrWhiteSpace(dto.Descripcion))
        {
            color.Descripcion = dto.Descripcion.SanitizarNombrePropio();
        }

        if(!string.IsNullOrWhiteSpace(dto.CodigoHex))
        {
            color.CodigoHex = dto.CodigoHex.Trim().ToUpperInvariant();
        }

        // 4. Almaceno los cambios detectados por el Change Tracker con el método .SaveChangesAsync().
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<ColorRespuestaDto> CrearColorAsync(CrearColorDto dto)
    {
        const string CODIGO_HEX_POR_DEFECTO = "FFFFFFFF"; // Blanco sólido.

        // 1. Comienzo a poblar la nueva instancia del modelo "Color"
        var hexLimpio = string.IsNullOrWhiteSpace(dto.CodigoHex) 
            ? CODIGO_HEX_POR_DEFECTO 
            : dto.CodigoHex.Trim().ToUpperInvariant();

        var nuevoColor = new Color
        {
            Descripcion = dto.Descripcion.SanitizarNombrePropio(),
            CodigoHex = hexLimpio
        };


        // 2. Alamaceno la nueva instancia en la BBDD.
        await _uow.Colores.AddAsync(nuevoColor);
        await _uow.SaveChangesAsync();

        // 3. Preparo una instancia de "ColorRespuestaDto" para enviársela al cliente.
        var respuesta = new ColorRespuestaDto
        {
            Id = nuevoColor.Id,
            Descripcion = nuevoColor.Descripcion,
            CodigoHex = nuevoColor.CodigoHex
        };

        return respuesta;
    }

    public async Task<bool> EliminarColorByIdAsync(int id)
    {
        // 1. Obtengo la entidad (modelo) que el usuario desea eliminar.
        var color = await _uow.Colores.GetByIdAsync(id);

        if(color is null) return false;

        // 2. Elimino el color particular usando la UoW para que el Change Tracker lo detecte.
        _uow.Colores.Remove(color);

        // 3. Almaceno los cambios realizados a la base de datos.
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminarColorByNombreAsync(string nombre)
    {
        // 1. Me cercioro que el usuario haya mandado un nombre válido para la búsqueda.
        if(string.IsNullOrWhiteSpace(nombre))
            // Imposible realizar la eliminación.
            return false;

        // 2. Limpio la cadena recibia para la búsqueda.
        var limpio = nombre.SanitizarNombrePropio();
        
        // 3. Obtengo la instancia específica de Color que el usuario desea eliminar.
        var colores = await _uow.Colores.GetAllAsync();
        var color = (colores ?? Enumerable.Empty<Color>())
            .FirstOrDefault(c => string.Equals(c.Descripcion.SanitizarNombrePropio(), limpio, StringComparison.OrdinalIgnoreCase));

        if(color is null) return false;

        // 4. Ejecuto la operación de eliminar para que el Change Tracker la detecte.
        _uow.Colores.Remove(color);

        // 5. Almaceno los cambios detectados por EF Core.
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ColorRespuestaDto>> ObtenerTodosAsync()
    {
        // 1. Obtengo todas las instancias (registros) desde la BBDD.
        var colores = await _uow.Colores.GetAllAsync();

        return (colores ?? Enumerable.Empty<Color>())
            .Select(c => new ColorRespuestaDto
            {
                Id = c.Id,
                Descripcion = c.Descripcion,
                CodigoHex = c.CodigoHex
            });
    }
}