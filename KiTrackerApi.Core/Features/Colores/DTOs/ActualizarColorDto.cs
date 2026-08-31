using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Features.Colores.DTOs;

public class ActualizarColorDto
{
    [StringLength(70, MinimumLength = 2, ErrorMessage = "El nombre del color debe contener al menos 2 caracteres.")]
    public string? Descripcion { get; set; }
    [RegularExpression(@"^([0-9a-fA-F]{6}|[0-9a-fA-F]{8})$", ErrorMessage = "El código HEX del color debe poseer entre 6 y 8 caracteres alfanuméricos.")]
    public string? CodigoHex { get; set; }
}