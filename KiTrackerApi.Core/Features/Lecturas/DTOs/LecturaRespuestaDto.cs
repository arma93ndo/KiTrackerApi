namespace KiTrackerApi.Core.Features.Lecturas.DTOs;

public class LecturaRespuestaDto
{
    public int Id { get; init; }
    public int LuchadorId { get; init; }
    public int DispositivoId { get; init; }
    public long NivelKi { get; init; }
    public DateTime FechaLectura { get; init; }

    // Datos relacionales proyectados (aplanados).
    public string NombreLuchador { get; init; } = string.Empty;
    public string? NombreEspecie { get; init; }
}