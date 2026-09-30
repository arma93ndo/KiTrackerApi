using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using KiTrackerApi.Core.Extensions;
using KiTrackerApi.Core.Features.Luchadores;
using KiTrackerApi.Core.Features.Luchadores.DTOs;
using KiTrackerApi.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace KiTrackerApi.Endpoints;

public static class LuchadoresEndpoints
{
    // Registro de los endpoints, propiamente dichos.
    public static void MapLuchadoresEndpoints(this IEndpointRouteBuilder app)
    {
        // 1. Registro la "raíz" de mis rutas, que siempre será "/luchadores/"
        var grupo = app.MapGroup("/luchadores");

        // 2. Registro mis rutas individuales en base a mi raíz (grupo) (/luchadores/xxxxxx).
        grupo.MapGet("/", ObtenerTodos)
                .WithName("ObtenerTodosLosLuchadores")
                .WithTags("Luchadores")
                .WithSummary("Obtiene todos los luchadores existentes")
                .WithDescription("Obtiene una lista con todos los luchadores existentes y la devuelve como un objeto JSON")
                .Produces<LuchadorRespuestaDto>(StatusCodes.Status200OK);

        grupo.MapGet("/{id:int}", ObtenerPorId)
                .WithName("ObtenerLuchadorPorId")
                .WithTags("Luchadores")
                .WithSummary("Obtiene un luchador en específico por Id")
                .WithDescription("Obtiene un luchador específico, basándose en su Id y lo regresa como un JSON")
                .Produces<LuchadorRespuestaDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

        grupo.MapGet("/nombre/{nombre}", ObtenerPorNombre)
                .WithName("ObtenerLuchadorPorNombre")
                .WithTags("Luchadores")
                .WithSummary("Obtiene un luchador en específico basándose en su nombre")
                .WithDescription("Obtiene un luchador específico basándose en su nombre de pila y lo regresa como un JSON")
                .Produces<LuchadorRespuestaDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

        grupo.MapGet("/especie/{especieId:int}", ObtenerPorEspecie)
                .WithName("ObtenerLuchadoresDeUnaEspecie")
                .WithTags("Luchadores")
                .WithSummary("Obtiene todos los luchadores de una especie dada")
                .WithDescription("Obtiene a todos los luchadores de una especie especificada y devuelve la lista como un objeto JSON")
                .Produces<IEnumerable<LuchadorRespuestaDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

        grupo.MapPost("/", Crear).RequireAuthorization() // Requiere un token JWT válido.
                .WithName("CrearNuevoLuchador")
                .WithTags("Luchadores")
                .WithSummary("Crea un nuevo luchador en la base de datos")
                .WithDescription("Crea un nuevo luchador basándose en la información que envíes en el body de tu solicitud. Este endpoint requiere estar logueado con un JWT")
                .Produces<LuchadorRespuestaDto>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);

        grupo.MapPut("/{id:int}", ActualizarPorId).RequireAuthorization() // Requiere un token JWT válido.
                .WithName("ActualizarLuchadorPorId")
                .WithTags("Luchadores")
                .WithSummary("Actualiza un luchador existente mediante su Id")
                .WithDescription("Actualiza un luchador existente en la aplicación utilizando los datos que envíes en el body de tu solicitud. Este endpoint require estar logueado con un JWT")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest);

        grupo.MapDelete("/{id:int}", EliminarPorId).RequireAuthorization() // Requiere un token JWT válido.
                .WithName("EliminarLuchadorPorId")
                .WithTags("Luchadores")
                .WithSummary("Eliminar un luchador existente mediante su Id")
                .WithDescription("Elimina un luchador existente en la aplicación mediante su Id. Este endpoint require estar logueado con un JWT")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    // Definición de todos los handlers.
    static async Task<IResult> ObtenerTodos(ILuchadorService service)
    {
        // Obtengo todos los luchadores existentes en la BBDD.
        var luchadores = await service.ObtenerTodosAsync();

        return TypedResults.Ok(luchadores);
    }

    [Authorize]
    static async Task<IResult> Crear([FromBody] CrearLuchadorDto dto, ILuchadorService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Valido que los parámetros proporcionados sean existentes y válidos. De lo contrario,
        // los añado a una lista con el resumen de todos los hallazgos.
        if(string.IsNullOrWhiteSpace(dto.Nombre))
            // Nombre obligatorio inválido.
            parametrosInvalidos.Add(new ParametroInvalido("nombre", "No puede estar vacío."));
        
        // 2. Si se acumularon errores, los enlisto en un formato Problem Details para el cliente.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = $"Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };
            // Anexo el agregado de la lista de parámetros inválidos.
            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;

            return TypedResults.Problem(problemDetails);
        }

        // 3. Si todo está correcto, creo (y persisto) el nuevo recurso.
        var luchadorCreado = await service.CrearLuchadorAsync(dto);

        // Status: 201 Created.
        return TypedResults.Created($"/luchadores/{luchadorCreado.Id}", luchadorCreado);
    }

    static async Task<IResult> ObtenerPorId(int id, ILuchadorService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que los argumentos sean válidos.
        if(id <= 0)
            // Id inválido.
            parametrosInvalidos.Add(new ParametroInvalido("id", "Debe ser un entero mayor a cero."));

        // 2. Si se acumularon errores, los enlisto en un formato Problem Details para el cliente.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = $"Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };
            // Anexo el agregado de la lista de parámetros inválidos.
            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;

            return TypedResults.Problem(problemDetails);
        }

        // 3. Si todo está en orden, obtengo la información solicitada.
        var luchador = await service.ObtenerByIdAsync(id);

        // 4. Verifico si el recurso existe o no.
        if(luchador is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(luchador);
    }

    static async Task<IResult> ObtenerPorNombre(string nombre, ILuchadorService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que los parámetros recibidos sean válidos.
        if(string.IsNullOrWhiteSpace(nombre))
            // Nombre inválido.
            parametrosInvalidos.Add(new ParametroInvalido("nombre", "No puede estar vacío."));
        
        // 2. Si se acumularon errores, les doy formato en Problem Details y lo envío al cliente.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = $"Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };
            // Anexo el agregado de la lista de parámetros inválidos.
            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;

            return TypedResults.Problem(problemDetails);
        }

        // 3. En caso de que todo esté en orden, consigo la información solicitada.
        var luchador = await service.ObtenerByNombreAsync(nombre);

        // 4. Verifico si el recurso existe o no.
        if(luchador is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(luchador);
    }

    static async Task<IResult> ObtenerPorEspecie(int especieId, ILuchadorService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Valido que los parámetros enviados por el cliente sean válidos.
        if(especieId <= 0)
            // Id inválido.
            parametrosInvalidos.Add(new ParametroInvalido("especieId", "Debe ser un entero mayor a cero."));

        // 2. En caso de que existan problemas acumulados, les doy formato Problem Details para el cliente.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = $"Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };
            // Anexo el agregado de la lista de parámetros inválidos.
            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;

            return TypedResults.Problem(problemDetails);
        }

        // 3. Si todo está en orden, obtengo las instancias solicitadas.
        var luchadores = await service.ObtenerByEspecieIdAsync(especieId);

        return TypedResults.Ok(luchadores);
    }

    [Authorize]
    static async Task<IResult> ActualizarPorId(int id, ActualizarLuchadorDto dto, ILuchadorService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que los argumentos enviados por el cliente sean válidos.
        if(id <= 0)
            // Id inválido.
            parametrosInvalidos.Add(new ParametroInvalido("id", "Debe ser un entero mayor a cero."));

        if(string.IsNullOrWhiteSpace(dto.Nombre))
            // Nombre inválido.
            parametrosInvalidos.Add(new ParametroInvalido("nombre", "No puede estar vacío."));

        // 2. Si se acumularon errores, los enlisto en un formato Problem Details para el cliente.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = $"Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };
            // Anexo el agregado de la lista de parámetros inválidos.
            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;

            return TypedResults.Problem(problemDetails);
        }

        // 3. Si está todo correcto, modifico el recurso solicitado.
        await service.ActualizarByIdAsync(id, dto);
        
        // Status 204 No Content.
        return TypedResults.NoContent();
    }

    [Authorize]
    static async Task<IResult> EliminarPorId(int id, ILuchadorService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que los parámetros proporcionados sean válidos.
        if(id <= 0)
            // Id inválido.
            parametrosInvalidos.Add(new ParametroInvalido("id", "Debe ser un entero mayor que cero."));

        // 2. Si se acumularon errores, los enlisto en formato Problem Details para el cliente.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = $"Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };
            // Anexo el agregado de la lista de parámetros inválidos.
            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;

            return TypedResults.Problem(problemDetails);
        }

        // 3. Si todo está en orden, ejecuto la operación solicitada.
        await service.EliminarByIdAsync(id);

        return TypedResults.NoContent();
    }
}