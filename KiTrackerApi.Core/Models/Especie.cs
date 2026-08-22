using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Models;

public class Especie
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "El nombre de la especie es obligatorio.")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "El nombre de la especie debe contener entre 2 y 255 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;
    [Range(0.001, 100.0, ErrorMessage = "El multiplicador de fuerza de la especie debe ser un número real positivo.")]
    public double Multiplicador { get; set; } = 1.0; // Valor por omisión si no se define explícitamente un factor al crear la especie.
    public ICollection<Luchador> Luchadores { get; set; } = []; // Propiedad de navegación inversa (1:N, navegación de colección o "hacia abajo").
}