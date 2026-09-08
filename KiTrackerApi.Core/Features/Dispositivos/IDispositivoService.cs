using KiTrackerApi.Core.Features.Dispositivos.DTOs;

namespace KiTrackerApi.Core.Features.Dispositivos;

public interface IDispositivoService
{
    // Métodos para mutaciones (creación, actualización).
    Task<DispositivoRespuestaDto> CrearDispositivoAsync(CrearDispositivoDto dto);
    Task<DispositivoRespuestaDto> ActualizarByIdAsync(int id, ActualizarDispositivoDto dto);
    Task EliminarByIdAsync(int id);
    // Métodos de consulta.
    Task<DispositivoRespuestaDto> ObtenerByIdAsync(int id);
    Task<DispositivoRespuestaDto> ObtenerByFingerprintAsync(string fingerprint);
    Task<IEnumerable<DispositivoRespuestaDto>> ObtenerTodosConDetallesAsync();
    Task<IEnumerable<DispositivoRespuestaDto>> ObtenerByColorIdAsync(int colorId);
}