namespace KiTrackerApi.Core.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTime Expiracion) GenerarToken(string nombreDeUsuario, string email);
}