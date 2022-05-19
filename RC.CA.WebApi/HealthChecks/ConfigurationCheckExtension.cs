using Microsoft.Extensions.Diagnostics.HealthChecks;
using NT.CA.WebUiMvc.HealthChecks;
using RC.CA.SharedKernel.GuardClauses;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigurationCheckExtension
{
    public static IHealthChecksBuilder CheckConfigurationFileSettings(this IHealthChecksBuilder builder,
                                                                      string name,
                                                                      IConfiguration configuration,
                                                                      HealthStatus failureStatus,
                                                                      IEnumerable<string> tags = default)
    {
        //Application settings exist
        //
        return builder.Add(new HealthCheckRegistration($"{name}",
                                                         new ConfigurationCheck(configuration),
                                                         failureStatus,
                                                         tags));
    }
}
