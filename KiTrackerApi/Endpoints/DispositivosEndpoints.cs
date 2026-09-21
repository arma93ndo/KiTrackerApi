using KiTrackerApi.Core.Features.Dispositivos;
using KiTrackerApi.Core.Features.Dispositivos.DTOs;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace KiTrackerApi.Endpoints;

public static class DispositivosEndpoints
{
    // Registro los endpoints existentes.
    public static void MapDispositivosEndpoints(this IEndpointRouteBuilder app)
    {
        // 1. Registro la raíz (grupo) de endpoints, que siempre comenzará con "/dispositivos/".
        var grupo = app.MapGroup("/dispositivos");

        // 2. Registro cada endpoint individualmente.
        grupo.MapPost("/", Crear).RequireAuthorization(); // Requiere un token JWT válido.
        grupo.MapPut("/{id:int}", ActualizarPorId).RequireAuthorization(); // Requiere un token JWT válido.
        grupo.MapDelete("/{id:int}", EliminarPorId).RequireAuthorization(); // Requiere un token JWT válido.
        grupo.MapGet("/{id:int}", ObtenerPorId);
        grupo.MapGet("/fingerprint/{id:int}", ObtenerPorFingerprint); 
        grupo.MapGet("/", ObtenerTodos); 
        grupo.MapGet("/color/{colorId:int}", ObtenerPorColor);
    }

    // Definición de todos los handlers.
    [Authorize]
    static async Task<IResult> Crear([FromBody] CrearDispositivoDto dto, IDispositivoService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que todos los datos enviados por el cliente sean válidos.
        if(string.IsNullOrWhiteSpace(dto.Fingerprint))
            // Fingerprint inválido.
            parametrosInvalidos.Add(new ParametroInvalido("fingerprint", "No puede estar vacío."));

        if(string.IsNullOrWhiteSpace(dto.Tipo))
            // Tipo inválido.
            parametrosInvalidos.Add(new ParametroInvalido("tipo", "No puede estar vacío."));

        if(dto.ColorId <= 0)
            // Color Id inválido.
            parametrosInvalidos.Add(new ParametroInvalido("colorId", "Debe ser mayor que cero."));

        // 2. En caso de que se hayan acumulado errores, los enlisto y doy formato a Problem Details.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = "Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;
            return TypedResults.Problem(problemDetails);
        }

        // 3. Si todo está correcto, creo y persisto el nuevo recurso.
        var dispositivoCreado = service.CrearDispositivoAsync(dto);
        return TypedResults.Created($"/dispositivos/{dispositivoCreado.Id}", dispositivoCreado);
    }

    [Authorize]
    static async Task<IResult> ActualizarPorId(int id, ActualizarDispositivoDto dto, IDispositivoService service, HttpContext httpContext)
    {
        // 1. Verifico que los parámetros enviados por el cliente sean válidos.
        var parametrosInvalidos = new List<ParametroInvalido>();

        if(id <= 0)
            // Id inválido.
            parametrosInvalidos.Add(new ParametroInvalido("id", "Debe ser un entero mayor a cero."));

        if(dto.Tipo is not null && string.IsNullOrWhiteSpace(dto.Tipo))
            // Tipo instanciado pero vacío.
            parametrosInvalidos.Add(new ParametroInvalido("tipo", "No puede estar vacío"));

        if(dto.ModeloHardware is not null && string.IsNullOrWhiteSpace(dto.ModeloHardware))
            // Modelo instanciado pero vacío.
            parametrosInvalidos.Add(new ParametroInvalido("modeloHardware", "No puede estar vacío"));

        if(dto.ColorId is not null && dto.ColorId <= 0)
            // ColorId instanciado pero inválido.
            parametrosInvalidos.Add(new ParametroInvalido("colorId", "Debe ser un entero mayor a cero."));
        
        // 2. En caso de que hayan habido problemas de validación, los enlisto con formato Problem Details.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = "Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;
            return TypedResults.Problem(problemDetails);
        }

        // 3. Si todo está en orden, modifico la instancia solicitada.
        await service.ActualizarByIdAsync(id, dto);

        return TypedResults.NoContent();
    }

    [Authorize]
    static async Task<IResult> EliminarPorId(int id, IDispositivoService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Me aseguro de que todos los argumentos sean válidos.
        if(id <= 0)
            // Id inválido.
            parametrosInvalidos.Add(new ParametroInvalido("id", "Debe ser mayor que cero."));

        // 2. Si hay algún problema, lo enlisto con formato Problem Details para el cliente.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = "Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;
            return TypedResults.Problem(problemDetails);
        }

        // 3. Si todo está correcto, hago la eliminación.
        await service.EliminarByIdAsync(id);
        
        return TypedResults.NoContent();
    }

    static async Task<IResult> ObtenerPorId(int id, IDispositivoService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Me aseguro de que todos los parámetros sean válidos.
        if(id <= 0)
            // Id inválido.
            parametrosInvalidos.Add(new ParametroInvalido("id", "Debe ser un entero mayor a cero."));

        // 2. En caso de que hubieran errores, los enlisto con formato Problem Details.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = "Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;
            return TypedResults.Problem(problemDetails);
        }

        // 3. Si todo estuvo correcto, obtengo los datos solicitados.
        var dispositivo = await service.ObtenerByIdAsync(id);

        return TypedResults.Ok(dispositivo);
    }

    static async Task<IResult> ObtenerPorFingerprint(string fingerprint, IDispositivoService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que todos los parámetros sean válidos.
        if(string.IsNullOrWhiteSpace(fingerprint))
            // Fingerprint inválido.
            parametrosInvalidos.Add(new ParametroInvalido("fingerprint", "No puede estar vacío."));

        // 2. En caso de haberlos, enlisto todos los errores con el formato Problem Details.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = "Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;
            return TypedResults.Problem(problemDetails);
        }
    
        // 3. Si no hubieron errores, obtengo la información solicitada.
        var dispositivo = await service.ObtenerByFingerprintAsync(fingerprint);

        return TypedResults.Ok(dispositivo);
    }

    static async Task<IResult> ObtenerTodos(IDispositivoService service, HttpContext httpContext)
    {
        var dispositivos = await service.ObtenerTodosConDetallesAsync();

        return TypedResults.Ok(dispositivos ?? Enumerable.Empty<DispositivoRespuestaDto>());
    }

    static async Task<IResult> ObtenerPorColor(int colorId, IDispositivoService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que todos los parámetros sean válidos.
        if(colorId <= 0)
            // colorId inválido.
            parametrosInvalidos.Add(new ParametroInvalido("colorId", "Debe ser un entero mayor que cero."));

        // 2. En caso de haber errores, los enlisto.
        if(parametrosInvalidos.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Error de validación",
                Detail = "Los parámetros enviados son inválidos. Revise la lista recibida.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["invalidParams"] = parametrosInvalidos;
            return TypedResults.Problem(problemDetails);
        }

        // 3. En caso de estar correctos, obtengo los datos solicitados.
        var dispositivos = await service.ObtenerByColorIdAsync(colorId);

        return TypedResults.Ok(dispositivos ?? Enumerable.Empty<DispositivoRespuestaDto>());
    }
}