using KiTrackerApi.Core.Features.Especies;
using KiTrackerApi.Core.Features.Especies.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KiTrackerApi.Endpoints;

public static class EspeciesEndpoints
{
    // Registro los endpoints uno por uno.
    public static void MapEspeciesEndpoints(this IEndpointRouteBuilder app)
    {
        // 1. Registro mi "raíz" (grupo) de mis rutas, que siempre será "/especies/".
        var grupo = app.MapGroup("/especies");

        // 2. Registro mis rutas (endpoints) individuales tomando como base mi raíz/grupo (/colores/xxxxx).
        grupo.MapGet("/", ObtenerTodos)
                .WithName("ObtenerTodasLasEspecies")
                .WithTags("Especies")
                .WithSummary("Obtiene todos los colores disponibles")
                .WithDescription("Obtiene una lista con todos los colores existentes en la aplicación y la devuelve como un objeto JSON")
                .Produces<EspecieRespuestaDto>(StatusCodes.Status200OK);
    }

    // Defino todos los handlers.
    static async Task<IResult> ObtenerTodos(IEspecieService service)
    {
        // Obtengto todas las especies existentes en la BBDD.
        var especies = await service.ObtenerTodasAsync();

        return TypedResults.Ok(especies);
    }
}