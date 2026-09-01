using KiTrackerApi.Core.Features.Luchadores.DTOs;

namespace KiTrackerApi.Core.Features.Luchadores;

public interface ILuchadorService
{
    Task<IEnumerable<LuchadorRespuestaDto>> ObtenerTodosAsync();
    Task<LuchadorRespuestaDto?> ObtenerByIdAsync(int id);
    Task<LuchadorRespuestaDto?> ObtenerByNombreAsync(string nombre);
    Task<LuchadorRespuestaDto> CrearLuchadorAsync(CrearLuchadorDto dto);
    Task<bool> ActualizarLuchadorByIdAsync(int id, ActualizarLuchadorDto dto);
    Task<bool> ActualizarLuchadorByNombreAsync(string nombre, ActualizarLuchadorDto dto);
    Task<bool> EliminarLuchadorByIdAsync(int id);
    Task<bool> EliminarLuchadorByNombreAsync(string nombre);
    Task<IEnumerable<LuchadorRespuestaDto>> ObtenerByEspecieIdAsync(int especieId);
}