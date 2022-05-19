using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NT.CA.WebUiMvc.HealthChecks;

public class ConfigurationCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private IReadOnlyDictionary<string, object> _healthCheckData;
    public ConfigurationCheck(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var cType = _configuration.GetValue<string>("ConfigType");
            if(string.IsNullOrEmpty(cType))
                return Task.FromResult(HealthCheckResult.Degraded($"ConfigType setting not found", null, _healthCheckData));

            var cSection = _configuration.GetSection("BlobStorage");
            if (cSection == null)
                return Task.FromResult(HealthCheckResult.Degraded($"BlobStorage setting not found", null, _healthCheckData));

            var corsSection = _configuration.GetSection("CorsSettings");
            if (cSection == null)
                return Task.FromResult(HealthCheckResult.Degraded($"CorsSettings setting not found", null, _healthCheckData));

            return Task.FromResult(HealthCheckResult.Healthy($"Configuration file '{cType}' found all settings configured"));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Degraded($"Exception configuration file check", ex, _healthCheckData));
        }
    }
}
