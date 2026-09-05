using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Features.Dispositivos.DTOs;

public class CrearDispositivoDto
{
    [Required(ErrorMessage = "El fingerprint del dispositivo es obligatorio.")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "El fingerprint del dispositivo debe contener por lo menos 2 caracteres.")]
    public string Fingerprint { get; init; } = string.Empty;
    [Required(ErrorMessage = "El tipo de dispositivo es obligatorio.")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "El tipo de dispositivo debe contener entre 2 y 60 caracteres.")]
    public string Tipo { get; init; } = string.Empty;
    [StringLength(80, MinimumLength = 1, ErrorMessage = "El modelo del dispositivo debe contener entre 1 y 80 caracteres.")]
    public string? ModeloHardware { get; init; }
    [Required(ErrorMessage = "El Id del color del dispositivo es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El Id del color asignado debe ser un número positivo mayor a cero.")]
    public int ColorId { get; init; }
}