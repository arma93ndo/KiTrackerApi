using KiTrackerApi.Core.Common.DTOs;
using KiTrackerApi.Services;
using Microsoft.AspNetCore.Identity;

namespace KiTrackerApi.Endpoints;

// Los endpoints de autorización "/auth" representan a su propio grupo. No a algún
// recurso existente en particular.
// Por lógica, no puedes exigir un token para pedir un token. Son la puerta de 
// entrada al sistema.
public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/auth");

        grupo.MapPost("/registrar", Registrar);
        grupo.MapPost("/login", Login);
    }

    static async Task<IResult> Registrar(RegistrarUsuarioDto dto, UserManager<IdentityUser> userManager)
    {
        var nuevoUsuario = new IdentityUser
        {
            UserName = dto.Email, // Usaré el email del usuario como nombre de usuario sin distinción.
            Email = dto.Email
        };

        // El método .CreateAsync() de Identity saca un hash de la contraseña antes de guardarla (la
        // contraseña nunca se almacena en el servidor).
        var resultado = await userManager.CreateAsync(nuevoUsuario, dto.Password);

        if(!resultado.Succeeded)
        {
            // En caso de que la contraseña proporcionada no cumpla con las reglas de Identity o que
            // el email esté duplicado en la BBDD, devolvemos un error 400 con la información relacionada
            // en formato Problem Details.
            var errores = resultado.Errors.ToDictionary(
                e => e.Code,
                e => new[] { e.Description }
            );

            return TypedResults.ValidationProblem(errores, title: "No se pudo registrar el usuario.");
        }

        // En caso de éxito llega hasta aquí. Debemos devolver un elocuente 201 ya que la operación de
        // creación de un recurso fue un éxito. Al crear un nuevo usuario no hace falta devolver un token,
        // eso se hace en el login.
        return TypedResults.Created($"/auth/usuarios/{nuevoUsuario.Id}", new
        {
            nuevoUsuario.Id,
            nuevoUsuario.Email
        });
    }

    static async Task<IResult> Login(LoginUsuarioDto dto,
                                        UserManager<IdentityUser> userManager,
                                        JwtTokenService tokenService)
    {
        var usuario = await userManager.FindByEmailAsync(dto.Email);

        // El método .CheckPasswordAsync() compara el hash de la contraseña proporcionada. El sistema
        // nunca conoce tu contraseña original. Sólo se guardó el hash al momento del registro.
        var credencialesValidas = usuario is not null 
                                    && await userManager.CheckPasswordAsync(usuario, dto.Password);

        if(!credencialesValidas)
        {
            // El mensaje del error es vago a propósito, ya que no mencionamos si lo que estuvo mal
            // fue el correo o la contraseña. Mencionar este dato le daría a un atacante maneras de
            // averiguar qué cuentas efectivamente existen.
            return TypedResults.Problem(
                title: "Credenciales inválidas.",
                detail: "El email o la contraseña no son correctos.",
                statusCode: StatusCodes.Status401Unauthorized
            );
        }

        // Identity ya validó que el usuario es quien dice ser. Por lo tanto, ahora requerimos de
        // un token JWT para que pueda demostrar la validez de su identidad.
        var (token, fechaDeExpiracion) = tokenService.GenerarToken(dto.Email, dto.Email);

        return TypedResults.Ok(new TokenRespuestaDto
        {
            Token = token,
            FechaExpiracion = fechaDeExpiracion
        });
    }
}