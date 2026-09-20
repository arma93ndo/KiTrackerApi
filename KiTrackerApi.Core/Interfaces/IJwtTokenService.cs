namespace KiTrackerApi.Core.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTime Expiracion) GenerarToken(string usuarioId, string email, string nombreUsuario);
}