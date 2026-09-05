namespace KiTrackerApi.Core.Features.Dispositivos.DTOs;

public class DispositivoRespuestaDto
{
    public int Id { get; init; }
    public string Fingerprint { get; init; } = string.Empty;
    public string Tipo { get; init; } = string.Empty;
    public DateTime FechaUltimoUso { get; init; }
    public int ColorId { get; init; }
    // Datos relacionales proyectados (aplanados).
    public string NombreColor { get; init; } = string.Empty;
}