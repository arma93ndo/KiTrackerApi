using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KiTrackerApi.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace KiTrackerApi.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuracion;

    public JwtTokenService(IConfiguration configuracion)
    {
        _configuracion = configuracion;
    }

    public (string Token, DateTime Expiracion) GenerarToken(string usuarioId, string email, string nombreUsuario)
    {
        // Los claims son lo que el token afirma (pares clave-valor) sobre el usuario.
        // El Jti (JSON Web Token Identifier) identifica a este token en concreto. Sirve para poder
        // revocarlo en el futuro con precisión.
        // El payload no está cifrado nunca en JWT.
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuarioId),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // La clave de la firma (para poder producir los JWT) proviene de la configuración, que en
        // el caso del entorno de desarrollo son los User Secrets. Si falta este secreto, preferimos
        // tronar la aplicación con un mensaje claro antes que arrancarla con una clave vacía.
        var claveJwt = _configuracion["Jwt:Clave"] ?? throw new InvalidOperationException("Falta la clave de firma del JWT." +
        "Configúrala por favor con: dotnet user-secrets set \"Jwt:Clave\" \"<clave de 32+ caracteres>\".");

        var llaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveJwt));
        // La firma no esconde el contenido, lo protege de modificaciones. Y como sólo el servidor
        // conoce la clave, nadie más puede fabricar tokens.
        var credenciales = new SigningCredentials(llaveSimetrica, SecurityAlgorithms.HmacSha256);

        // Un token sin expiración es un riesgo permanente. La única manera de invalidarlo sería
        // cambiar la clave de toda la API, lo cual perjudicaría a los tokens de todos los usuarios.
        var minutos = _configuracion.GetValue("Jwt:ExpiracionEnMinutos", 90);
        var fechaExpiracion = DateTime.UtcNow.AddMinutes(minutos);

        var token = new JwtSecurityToken(
            issuer: _configuracion["Jwt:Issuer"],
            audience: _configuracion["Jwt:Audience"],
            claims: claims,
            expires: fechaExpiracion,
            signingCredentials: credenciales
        );

        // El método .WriteToken() serializa las 3 partes (encabezados.payload.firma) al 
        // string (JWT) resultante.
        return (new JwtSecurityTokenHandler().WriteToken(token), fechaExpiracion);
    }
}