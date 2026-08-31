namespace KiTrackerApi.Core.Features.Colores.DTOs;

public class ColorRespuestaDto
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? CodigoHex { get; set; }
}