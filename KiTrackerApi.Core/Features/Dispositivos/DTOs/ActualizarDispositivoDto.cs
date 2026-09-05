using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Features.Dispositivos.DTOs;

public class ActualizarDispositivoDto
{
    public string? Tipo { get; init; }
    public string? ModeloHardware { get; init; }
    [Range(1, int.MaxValue, ErrorMessage = "El Id del color asignado debe ser un número positivo mayor a cero.")]
    public int? ColorId { get; init; }
}