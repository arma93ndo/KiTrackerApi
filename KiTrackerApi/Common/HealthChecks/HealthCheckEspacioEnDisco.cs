using Microsoft.Extensions.Diagnostics.HealthChecks;
using KiTrackerApi.Core.Constants;

namespace KiTrackerApi.Common.HealthChecks;

public class HealthCheckEspacioEnDisco : IHealthCheck
{
    private readonly ILogger<HealthCheckEspacioEnDisco> _logger;

    public HealthCheckEspacioEnDisco(ILogger<HealthCheckEspacioEnDisco> logger)
    {
        _logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // Obtengo información del disco donde corre esta app.
        var volumen = new DriveInfo(Directory.GetCurrentDirectory());

        // Convierto el espacio disponible a Megabytes.
        var megabytesLibres = volumen.AvailableFreeSpace / (1024 * 1024);

        if(megabytesLibres < ReglasHealthChecks.ESPACIO_EN_DISCO_CRITICO_MB)
        {
            // Espacio en disco críticamente bajo.
            _logger.LogError("HealthCheck fallido. Espacio en disco críticamente bajo: {MegaBytesLibres} MB disponibles.", megabytesLibres);
            return Task.FromResult(HealthCheckResult.Unhealthy($"Espacio en disco crítico: {megabytesLibres} MB disponibles."));
        }

        if(megabytesLibres < ReglasHealthChecks.ESPACIO_EN_DISCO_DISMINUIDO_MB)
        {
            // Espacio en disco disminuido.
            _logger.LogWarning("HealthCheck en riesgo. Espacio en disco bajo: {MegaBytesLibres} MB disponibles.", megabytesLibres);
            return Task.FromResult(HealthCheckResult.Degraded($"Espacio en disco bajo: {megabytesLibres} MB disponibles."));
        }

        // Es espacio en disco duro es más que suficiente.
        _logger.LogInformation("Verificación del disco exitosa. Megabytes disponibles: {MegabytesLibres} MB.", megabytesLibres);
        return Task.FromResult(HealthCheckResult.Healthy($"Espacio en disco suficiente."));
    }
}