using KiTrackerApi.Core.Features.Especies.DTOs;

namespace KiTrackerApi.Core.Features.Especies;

public interface IEspecieService
{
    Task<EspecieRespuestaDto> CrearEspecieAsync(CrearEspecieDto dto);
    Task<IEnumerable<EspecieRespuestaDto>> ObtenerTodasAsync();
    Task<bool> ActualizarEspecieByIdAsync(int id, ActualizarEspecieDto dto); // La actualización tuvo éxito o no.
    Task<bool> ActualizarEspecieByNombreAsync(string nombre, ActualizarEspecieDto dto); // La actualización tuvo éxito o no.
    Task<bool> EliminarEspecieByIdAsync(int id); // Devolver el objeto borrado consume ancho de banda innecesariamente.
    Task<bool> EliminarEspecieByNombreAsync(string nombre); // Devolver el objeto borrado consume ancho de banda innecesariamente.
}