using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Models;

public class Color
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "La descripción del color es obligatoria.")]
    [StringLength(70, MinimumLength = 2, ErrorMessage = "El nombre del color debe contener al menos 2 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;
    [Required(ErrorMessage = "El código HEX (R, G, B, Alpha) del color es obligatorio.")]
    [StringLength(8, MinimumLength = 6, ErrorMessage = "El código HEX del color debe poseer entre 6 y 8 caracteres alfanuméricos.")]
    public string CodigoHex { get; set; } = string.Empty;
    public ICollection<Dispositivo> Dispositivos { get; set; } = [];
}