using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Models;

public class Lectura
{
    [Key]
    public int Id { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "El ID del luchador debe ser positivo.")] // Seguridad referencial en una llave foránea.
    public int LuchadorId { get; set; } // Llave foránea (FK). Identifica a quién pertenece una lectura específica.
    public Luchador? Luchador { get; set; } = null!;// Propiedad de navegación (relación N:1 con Luchador).
    [Range(1, int.MaxValue, ErrorMessage = "El ID del dispositivo debe ser positivo.")] // Seguridad referencial en una llave foránea.
    public int DispositivoId { get; set; } // Llave foránea (FK). Identifica qué dispositivo tomó la captura.
    public Dispositivo? Dispositivo { get; set; } = null!; // Propiedad de navegación (relación 1:N con Dispositivo).
    [Required(ErrorMessage = "El puntaje de ki es obligatorio.")]
    [Range(0, long.MaxValue, ErrorMessage = "El nivel del ki no puede ser un valor negativo.")]
    public long NivelKi { get; set; } // El número calculado de unidades de energía. Se usa long porque los valores
    // crecen exponencialmente.
    [RegularExpression(@"^(?i)[\w\-. /]+\.(jpg|jpeg|png|webp|svg)$", ErrorMessage = "Debes ingresar una ruta de imagen válida.")]
    [StringLength(512, ErrorMessage = "La ruta de la foto no puede exceder los 512 caracteres.")]
    public string? RutaFotografia { get; set; } = string.Empty; // Ruta en el servidor del archivo de la foto procesada/almacenada.
    public DateTime FechaLectura { get; set; } = DateTime.UtcNow; // Timestamp en UTC de cuándo se tomó la lectura.
}