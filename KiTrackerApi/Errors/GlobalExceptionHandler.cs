using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace KiTrackerApi.Errors;

// IExceptionHandler atrapa cualquier excepción no manejada del pipeline y gracias al estándar
// ProblemDetails, las convierte en una respuesta consistente.
public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Los detalles reales sobre la excepción serán registrados dentro del servidor en un .log. Jamás viajarán al cliente.
        logger.LogError(exception, "Excepción no manejada al procesar {Metodo} {Ruta}. Mensaje: {Mensaje}\nStack trace: {StackTrace}", httpContext.Request.Method, // Logging estructurado.
            httpContext.Request.Path, exception.Message, exception.StackTrace);

        // Utilizo una expresión switch (pattern matching) para determinar qué tipo de excepción ocurrió.
        var (statusCode, title) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado."),
            ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, "Parámetro fuera de rango."),
            ArgumentException => (StatusCodes.Status400BadRequest, "Solicitud con parámetros inválidos."),
            InvalidOperationException => (StatusCodes.Status409Conflict, "Conflicto detectado al realizar la operación."),
            HttpRequestException => (StatusCodes.Status500InternalServerError, "La llamada (solicitud) HTTP falló."),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor.")

        };

        // Comunico el tipo de fallo al contexto HTTP.
        httpContext.Response.StatusCode = statusCode;

        // La respuesta al cliente no incluirá ni el mensaje interno, ni el "stack trace", sólo un texto genérico (como
        // debe ser).
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Title = title,
                Detail = "El servidor no pudo procesar la solicitud. Intente de nuevo más tarde",
                Status = statusCode,
                Instance = httpContext.Request.Path
            }
        });
    }
}