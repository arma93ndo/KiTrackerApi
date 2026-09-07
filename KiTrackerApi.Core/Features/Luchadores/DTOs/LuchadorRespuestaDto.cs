using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Features.Luchadores.DTOs;
public class LuchadorRespuestaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int EspecieId { get; set; }
    public string NombreEspecie { get; set; } = string.Empty;
}