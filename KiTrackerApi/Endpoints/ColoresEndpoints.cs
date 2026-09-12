using KiTrackerApi.Core.Features.Colores;

namespace KiTrackerApi.Endpoints;

public static class ColoresEndpoints
{
    // Registro los endpoints uno por uno.
    public static void MapColoresEndpoints(this IEndpointRouteBuilder app)
    {
        // 1. Registro la "raíz" de mis rutas, que siempre será "/colores/".
        var grupo = app.MapGroup("/colores");

        // 2. Registro mis rutas (endpoints) individuales tomando como base mi raíz/grupo (/colores/xxxxx).
        grupo.MapGet("/", ObtenerTodos);
    }

    // Defino todos los handlers.
    static async Task<IResult> ObtenerTodos(IColorService service)
    {
        // Obtengo todos los colores existentes en la BBDD.
        var colores = await service.ObtenerTodosAsync();

        return TypedResults.Ok(colores);
    }
}