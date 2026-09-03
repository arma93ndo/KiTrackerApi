using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Features.Lecturas.DTOs;

public class CrearLecturaDto
{
    [Required(ErrorMessage = "El ID del luchador leído es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del luchador debe ser mayor que cero.")]
    public int LuchadorId { get; set; }
    [Required(ErrorMessage = "El fingerprint del dispositivo es obligatorio.")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "El fingerprint del dispositivo debe contener por lo menos 2 caracteres.")]
    public string FingerprintDispositivo { get; set; } = string.Empty;
    [Required(ErrorMessage = "El nivel de ki medido es obligatorio.")]
    [Range(0, long.MaxValue, ErrorMessage = "El nivel de ki no puede ser un valor negativo")]
    public long NivelKi { get; set; }
    [RegularExpression(@"^(?i)[\w\-. /]+\.(jpg|jpeg|png|webp|svg)$", ErrorMessage = "Debes ingresar una ruta de imagen válida.")]
    [StringLength(512, ErrorMessage = "La ruta de la foto no puede exceder los 512 caracteres.")]
    public string? RutaFotografia { get; set; }
}