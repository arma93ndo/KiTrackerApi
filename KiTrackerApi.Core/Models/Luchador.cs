using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiTrackerApi.Core.Models;

public class Luchador
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "El nombre del luchador es obligatorio.")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "El nombre del luchador debe contener al menos 1 caracter.")]
    public string Nombre { get; set; } = string.Empty;
    public int EspecieId { get; set; } // Clave foránea (detectada por convención de EF).
    public Especie? Especie { get; set; } // Propiedad de navegación (para poder usar .Include() y eso).
    [RegularExpression(@"^(?i)[\w\-. /]+\.(jpg|jpeg|png|webp|svg)$", ErrorMessage = "Debes ingresar una ruta de imagen válida.")]
    public string? FotoPerfilUrl { get; set; } // Almacena la ruta de la imagen que sirve como avatar del sujeto medido.
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;// Timestamp en UTC de cuando se dio de alta al luchador en el sistema.
    public ICollection<Lectura> Lecturas { get; set; } = []; // Propiedad de navegación inversa (1:N) (un luchador puede haber sido leído muchas veces).
}