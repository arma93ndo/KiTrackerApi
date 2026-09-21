using KiTrackerApi.Core.Features.Lecturas;
using KiTrackerApi.Core.Features.Lecturas.DTOs;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Errors;
using Microsoft.AspNetCore.Mvc;
using KiTrackerApi.Core.Constants;
using KiTrackerApi.Common.DTOs;
using KiTrackerApi.Common.Constants;
using Microsoft.AspNetCore.Authorization;

namespace KiTrackerApi.Endpoints;

public static class LecturasEndpoints
{
    public static void MapLecturasEndpoints(this IEndpointRouteBuilder app)
    {
        // 1. Registro el grupo "/lecturas" que concentrará todos los endpoints relacionados con Lecturas.
        var grupo = app.MapGroup("/lecturas");

        // 2. Registro cada endpoint existente uno por uno.
        grupo.MapGet("/", ObtenerTodas);
        grupo.MapGet("/{id:int}", ObtenerPorId);
        grupo.MapPost("/", Crear).RequireAuthorization(); // Exige un token JWT válido.
        grupo.MapDelete("/{id:int}", EliminarPorId).RequireAuthorization(); // Con esta sola llamada, el endpoint
        // exige un token válido.
        grupo.MapGet("/especie/{especieId:int}", ObtenerPorEspecie);
        grupo.MapGet("/luchador/{luchadorId:int}", ObtenerPorLuchador);
        grupo.MapGet("/nombre/{nombreLuchador}", ObtenerPorNombreLuchador);
        grupo.MapGet("/max/{top:int}", ObtenerMaximos);
        grupo.MapGet("/rango", ObtenerPorRango);
    }

    static async Task<IResult> ObtenerPorId(int id, ILecturaService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Valido que todos los argumentos sean válidos.
        if(id <= 0)
            // Id inválido.
            parametrosInvalidos.Add(new ParametroInvalido("id", "Debe ser un entero mayor a cero."));

        // 2. En caso de haber errores, los enlisto con el formato Problem Details.
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

        // 3. Si todo está en orden, regreso los datos solicitados.
        var lectura = await service.ObtenerByIdAsync(id);

        return TypedResults.Ok(lectura);
    }

    [Authorize]
    static async Task<IResult> Crear([FromBody] CrearLecturaDto dto, ILecturaService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que todos los argumentos sean válidos.
        if(dto.LuchadorId <= 0)
            // Id de luchador inválido.
            parametrosInvalidos.Add(new ParametroInvalido("luchadorId", "Debe ser un entero mayor a cero."));
        
        if(string.IsNullOrWhiteSpace(dto.FingerprintDispositivo))
            // Fingerprint inválido.
            parametrosInvalidos.Add(new ParametroInvalido("fingerprintDispositivo", "No debe estar vacío."));

        if(dto.NivelKi < 0)
            // Nivel de ki inválido.
            parametrosInvalidos.Add(new ParametroInvalido("nivelKi", "El nivel de ki no puede ser negativo."));

        if(dto.RutaFotografia is not null && string.IsNullOrWhiteSpace(dto.RutaFotografia))
            // RutaFotografía instanciada pero inválida.
            parametrosInvalidos.Add(new ParametroInvalido("rutaFotografia", "No puede estar vacía."));

        // 2. En caso de haber errores, los enlisto usando el formato Problem Details.
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

        // 3. Si todo está en orden, instancio y persisto el nuevo recurso.
        var lecturaNueva = await service.CrearLecturaAsync(dto);

        // 4. Devuelvo una representación del a nueva instancia junto con su URL específica.
        return TypedResults.Created($"/lecturas/{lecturaNueva.Id}", lecturaNueva);
    }

    [Authorize]
    static async Task<IResult> EliminarPorId(int id, ILecturaService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que los argumentos sean válidos.
        if(id <= 0)
            // Id inválido.
            parametrosInvalidos.Add(new ParametroInvalido("id", "Debe ser un entero mayor a cero."));
        
        // 2. Si hubieron errores, los enlisto en formato Problem Details.
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

        // 3. Si todo está en orden, ejecuto la eliminación solicitada.
        await service.EliminarByIdAsync(id);

        return TypedResults.NoContent();
    }

    static async Task<IResult> ObtenerPorEspecie(int especieId, ILecturaService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que todos los argumentos sean válidos.
        if(especieId <= 0)
            // especieId inválido.
            parametrosInvalidos.Add(new ParametroInvalido("especieId", "Debe ser un entero mayor a cero."));

        // 2. Si hubieron errores, los enlisto usando el formato Problem Details.
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

        // Si todo está correcto, consulto los datos solicitados.
        var lecturas = await service.ObtenerByEspecieIdAsync(especieId);

        return TypedResults.Ok(lecturas);
    }

    static async Task<IResult> ObtenerPorLuchador(int luchadorId, ILecturaService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Valido que todos los argumentos sean válidos.
        if(luchadorId <= 0)
            // luchadorId inválido.
            parametrosInvalidos.Add(new ParametroInvalido("luchadorId", "Debe ser un entero mayor a cero."));
        
        // 2. Si hubieron errores, los enlisto usando Problem Details.
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

        // 3. Si todo fue  correcto, obtengo los datos solicitados.
        var lecturas = await service.ObtenerByLuchadorIdAsync(luchadorId);

        return TypedResults.Ok(lecturas);
    }

    static async Task<IResult> ObtenerPorNombreLuchador(string nombreLuchador, ILecturaService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que todos los argumentos sean válidos.
        if(string.IsNullOrWhiteSpace(nombreLuchador))
            // nombreLuchador inválido.
            parametrosInvalidos.Add(new ParametroInvalido("nombreLuchador", "No puede estar vacío."));

        // 2. Si hubieron errores, los enlisto.
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

        // 3. Si todo está en orden, obtengo los datos solicitados.
        var lecturas = await service.ObtenerByNombreLuchadorAsync(nombreLuchador);

        return TypedResults.Ok(lecturas);
    }

    static async Task<IResult> ObtenerMaximos(int top, ILecturaService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico que los argumentos sean válidos.
        if(top < 0 || top > ReglasLectura.LIMITE_MAXIMO_TOP)
            // Número de máximos kis inválido.
            parametrosInvalidos.Add(new ParametroInvalido("top", $"Debe estar entre 1 y {ReglasLectura.LIMITE_MAXIMO_TOP}"));

        // 2. Si hubieron errores, los enlisto con formato Problem Details.
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

        // 3. Si todo es correcto, obtengo las lecturas solicitadas.
        var lecturas = await service.ObtenerMaximosKisAsync(top);

        return TypedResults.Ok(lecturas);
    }

    static async Task<IResult> ObtenerPorRango(long minimo, long maximo, ILecturaService service, HttpContext httpContext)
    {
        var parametrosInvalidos = new List<ParametroInvalido>();

        // 1. Verifico todos los parámetros de entrada.
        if(minimo < 0)
            // minimo inválido.
            parametrosInvalidos.Add(new ParametroInvalido("minimo", "El mínimo debe ser un entero igual o mayor a cero."));

        if(maximo < 0)
            // maximo inválido.
            parametrosInvalidos.Add(new ParametroInvalido("maximo", "El máximo debe ser un entero igual o mayor a cero."));

        if(maximo < minimo)
            // Rango inválido.
            parametrosInvalidos.Add(new ParametroInvalido("rango", "El máximo no puede ser menor que el mínimo."));

        // 2. Si hubieron errores los enlisto con formato Problem Details.
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

        // 3. Si todo está en orden, obtengo las lecturas solicitadas.
        var lecturas = await service.ObtenerByRangoDeKiAsync(minimo, maximo);

        return TypedResults.Ok(lecturas);
    }

    static async Task<IResult> ObtenerTodas(ILecturaService service, HttpContext httpContext,
        long? kiMinimo = null, long? kiMaximo = null, string? ordenarPor = null,
        bool descendente = false, int pagina = 1, int tamanioPagina = PaginacionConstantes.TAMANIO_PAGINA_POR_DEFECTO)
    {
        // 1. Me aseguro de sanitizar los tamaños de la paginación.
        (pagina, tamanioPagina) = NormalizarPaginacion(pagina, tamanioPagina);

        // 2. Obtengo solo la cantidad de datos solicitados desde la base de datos.
        var (datos, totalRegistros) = await service.FiltrarAsync(
            pagina: pagina,
            tamanioPagina: tamanioPagina,
            kiMinimo: kiMinimo,
            kiMaximo: kiMaximo,
            ordenarPor: ordenarPor,
            descendente: descendente);

        // 3. Devuelvo los datos en un DTO para el cliente.
        return TypedResults.Ok(new RespuestaPaginadaDto<LecturaRespuestaDto>
        {
            Datos = datos,
            Pagina = pagina,
            TamanioPagina = tamanioPagina,
            TotalRegistros = totalRegistros,
            TotalPaginas = (int) Math.Ceiling(totalRegistros / (double)tamanioPagina)
        });
    }

    // Normalizar número de pagína/tamaño de paǵina a valores sanos: página mínima 1, tamaño de página
    // entre 1 y un límite.
    static (int pagina, int TamanioPagina) NormalizarPaginacion(int pagina, int tamanioPagina)
    {
        if(pagina < 1) pagina = 1;
        if(tamanioPagina < 1) tamanioPagina = PaginacionConstantes.TAMANIO_PAGINA_POR_DEFECTO;
        if(tamanioPagina > PaginacionConstantes.TAMANIO_PAGINA_MAXIMO) tamanioPagina = PaginacionConstantes.TAMANIO_PAGINA_MAXIMO;
        return (pagina, tamanioPagina);
    }
}



