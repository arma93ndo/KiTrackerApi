using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Features.Colores.DTOs;

public class CrearColorDto
{
    [Required(ErrorMessage = "La descripción del color es obligatoria.")]
    [StringLength(70, MinimumLength = 2, ErrorMessage = "El nombre del color debe contener al menos 2 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;
    [RegularExpression(@"^([0-9a-fA-F]{6}|[0-9a-fA-F]{8})$", ErrorMessage = "El código HEX del color debe poseer entre 6 y 8 caracteres alfanuméricos.")]
    public string? CodigoHex { get; set; }
}