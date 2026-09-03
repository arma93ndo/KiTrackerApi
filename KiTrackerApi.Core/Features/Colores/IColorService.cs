using KiTrackerApi.Core.Models;
using KiTrackerApi.Core.Features.Colores.DTOs;

namespace KiTrackerApi.Core.Features.Colores;

public interface IColorService
{
    Task<ColorRespuestaDto> CrearColorAsync(CrearColorDto dto);
    Task<IEnumerable<ColorRespuestaDto>> ObtenerTodosAsync();
    Task ActualizarColorByIdAsync(int id, ActualizarColorDto dto);
    Task ActualizarColorByNombreAsync (string nombre, ActualizarColorDto dto);
    Task EliminarColorByIdAsync (int id);
    Task EliminarColorByNombreAsync (string nombre);
}