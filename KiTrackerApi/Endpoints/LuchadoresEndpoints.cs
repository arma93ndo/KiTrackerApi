using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using KiTrackerApi.Core.Extensions;
using KiTrackerApi.Core.Features.Luchadores;
using KiTrackerApi.Core.Features.Luchadores.DTOs;
using KiTrackerApi.Errors;
using Microsoft.AspNetCore.Mvc;

namespace KiTrackerApi.Endpoints;

public static class LuchadoresEndpoints
{
    // Registro de los endpoints, propiamente dichos.
    public static void MapLuchadoresEndpoints(this IEndpointRouteBuilder app)
    {
        // 1. Registro la "raíz" de mis rutas, que siempre será "/luchadores/"
        var grupo = app.MapGroup("/luchadores");

        // 2. Registro mis rutas individuales en base a mi raíz (grupo) (/luchadores/xxxxxx).
        grupo.MapGet("/", ObtenerTodos); 
        grupo.MapGet("/{id:int}", ObtenerPorId);
        grupo.MapGet("/nombre/{nombre}", ObtenerPorNombre);
        grupo.MapGet("/especie/{especieId:int}", ObtenerPorEspecie);
        grupo.MapPost("/", Crear);
        grupo.MapPut("/{id:int}", ActualizarPorId);
        grupo.MapDelete("/{id:int}", EliminarPorId);
    }

    // Definición de todos los handlers.
    static async Task<IResult> ObtenerTodos(ILuchadorService service)
    {
        // Obtengo todos los luchadores existentes en la BBDD.
        var luchadores = await service.ObtenerTodosAsync();

        return TypedResults.Ok(luchadores);
    }

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
        
        return TypedResults.NoContent();
    }

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