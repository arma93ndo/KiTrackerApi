using System.ComponentModel.DataAnnotations;

namespace KiTrackerApi.Core.Common.DTOs;

public class RegistrarUsuarioDto
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contraseña debe contener al menos 8 caracteres.")]
    public string Password { get; set; } = string.Empty;
}