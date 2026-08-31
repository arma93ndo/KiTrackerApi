using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Features.Especies.DTOs;

public class EspecieRespuestaDto
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
} 