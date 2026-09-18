using KiTrackerApi.Core.Features.Lecturas.DTOs;

namespace KiTrackerApi.Core.Features.Lecturas;

public interface ILecturaService
{
    Task<(IEnumerable<LecturaRespuestaDto>, int TotalRegistros)> FiltrarAsync(int pagina,
                                                                            int tamanioPagina,
                                                                            long? kiMinimo,
                                                                            long? kiMaximo,
                                                                            string? ordenarPor,
                                                                            bool descendente = false);
    Task<IEnumerable<LecturaRespuestaDto>> ObtenerTodasAsync();
    Task<LecturaRespuestaDto?> ObtenerByIdAsync(int id);
    Task<LecturaRespuestaDto> CrearLecturaAsync(CrearLecturaDto dto);
    Task EliminarByIdAsync(int id);
    Task<IEnumerable<LecturaRespuestaDto>> ObtenerByEspecieIdAsync(int especieId);
    Task<IEnumerable<LecturaRespuestaDto>> ObtenerByLuchadorIdAsync(int luchadorId);
    Task<IEnumerable<LecturaRespuestaDto>> ObtenerByNombreLuchadorAsync(string nombreLuchador);
    Task<IEnumerable<LecturaRespuestaDto>> ObtenerByRangoDeKiAsync(long minimo, long maximo);
    Task<IEnumerable<LecturaRespuestaDto>> ObtenerMaximosKisAsync(int top);   
}