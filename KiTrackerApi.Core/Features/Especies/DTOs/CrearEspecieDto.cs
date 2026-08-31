using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Features.Especies.DTOs;

public class CrearEspecieDto
{
    [Required(ErrorMessage = "El nombre de la especie es obligatorio.")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "El nombre de la especie debe contener entre 2 y 255 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;
    [Range(0.001, 100.0, ErrorMessage = "El multiplicador de fuerza de la especie debe ser un número real positivo de máximo 100.0.")]
    public double? Multiplicador { get; set; } // Se asume que el multiplicador es opcional para el cliente. null por omisión.
}