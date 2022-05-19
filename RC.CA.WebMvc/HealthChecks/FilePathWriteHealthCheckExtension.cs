using Microsoft.Extensions.Diagnostics.HealthChecks;
using NT.CA.WebUiMvc.HealthChecks;
using RC.CA.SharedKernel.GuardClauses;

namespace Microsoft.Extensions.DependencyInjection;

public static class FilePathWriteHealthCheckExtension
{
    //checked file path access logs and images
    //
    public static IHealthChecksBuilder AddFilePathWrite(this IHealthChecksBuilder builder, 
                                                        string name,string filePath,
                                                        HealthStatus failureStatus, 
                                                        IEnumerable<string> tags = default)
    {
        Guard.Against.NullOrEmpty(filePath, nameof(filePath));

        return builder.Add(new HealthCheckRegistration($"{name} {filePath}",
                                                        new FilePathWriteHealthCheck(filePath),
                                                        failureStatus,
                                                        tags));
    }
}
