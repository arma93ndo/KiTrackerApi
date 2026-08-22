using System.ComponentModel.DataAnnotations;


namespace KiTrackerApi.Core.Models;

public class Dispositivo
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "El identificador del dispositivo es obligatorio.")]
    public string Fingerprint { get; set; } = string.Empty;
    [Required(ErrorMessage = "El tipo de dispositivo es obligatorio.")]
    public string Tipo { get; set; } = string.Empty; // p. ej. "Mobile", "WebBrowser", etc.
    public string? ModeloHardware { get; set; } // p. ej. "Pixel 8", "iPhone 15", etc.
    public DateTime FechaUltimoUso { get; set; } = DateTime.UtcNow; // Timestamp de la última vez que el dispoisitivo se
    // comunicó con la API.
    public ICollection<Lectura> Lecturas { get; set; } = []; // Propiedad de navegación (relación 1:N con Lectura).
    public int ColorId { get; set; } // Clave foránea (detectada por convención de EF).
    public Color? Color { get; set; } // Propiedad de navegación (relación N:1 con Color).
}