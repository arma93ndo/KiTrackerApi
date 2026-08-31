using KiTrackerApi.Core.Models;
using KiTrackerApi.Core.Features.Colores.DTOs;

namespace KiTrackerApi.Core.Features.Colores;

public interface IColorService
{
    Task<ColorRespuestaDto> CrearColorAsync(CrearColorDto dto);
    Task<IEnumerable<ColorRespuestaDto>> ObtenerTodosAsync();
    Task<bool> ActualizarColorByIdAsync(int id, ActualizarColorDto dto);
    Task<bool> ActualizarColorByNombreAsync (string nombre, ActualizarColorDto dto);
    Task<bool> EliminarColorByIdAsync (int id);
    Task<bool> EliminarColorByNombreAsync (string nombre);
}