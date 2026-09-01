using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Features.Luchadores.DTOs;

public class CrearLuchadorDto
{
    [Required(ErrorMessage = "El nombre del luchador es obligatorio.")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "El nombre del luchador debe contener al menos 1 caracter.")]
    public string Nombre { get; set; } = string.Empty;
    [Required(ErrorMessage = "El ID de especie del luchador debe ser proporcionado.")]
    [Range(1, int.MaxValue, ErrorMessage = "El valor del ID de la especie debe ser un número entero mayor que cero.")]
    public int EspecieId { get; set; }
    [RegularExpression(@"^(?i)[\w\-. /]+\.(jpg|jpeg|png|webp|svg)$", ErrorMessage = "Debes ingresar una ruta de imagen válida.")]
    [StringLength(512, ErrorMessage = "La ruta de la imagen no puede exceder los 512 caracteres.")]
    public string? FotoPerfilUrl { get; set; }
}