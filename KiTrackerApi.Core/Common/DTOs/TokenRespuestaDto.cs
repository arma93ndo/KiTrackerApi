namespace KiTrackerApi.Core.Common.DTOs;

public class TokenRespuestaDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime FechaExpiracion { get; set; }
}