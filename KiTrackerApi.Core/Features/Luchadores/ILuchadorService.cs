using KiTrackerApi.Core.Features.Luchadores.DTOs;

namespace KiTrackerApi.Core.Features.Luchadores;

public interface ILuchadorService
{
    Task<IEnumerable<LuchadorRespuestaDto>> ObtenerTodosAsync();
    Task<LuchadorRespuestaDto?> ObtenerByIdAsync(int id);
    Task<LuchadorRespuestaDto?> ObtenerByNombreAsync(string nombre);
    Task<LuchadorRespuestaDto> CrearLuchadorAsync(CrearLuchadorDto dto);
    Task ActualizarByIdAsync(int id, ActualizarLuchadorDto dto);
    Task ActualizarByNombreAsync(string nombre, ActualizarLuchadorDto dto);
    Task EliminarByIdAsync(int id);
    Task EliminarByNombreAsync(string nombre);
    Task<IEnumerable<LuchadorRespuestaDto>> ObtenerByEspecieIdAsync(int especieId);
}