using Azure.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NT.CA.WebUiApi.HealthChecks;
using NT.CA.WebUiMvc.HealthChecks;
using RC.CA.SharedKernel.GuardClauses;

namespace Microsoft.Extensions.DependencyInjection;

public static class AzureKeyVaultHealthCheckExtension
{
    /// <summary>
    /// Check azure key vault for list of keys
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name"></param>
    /// <param name="keyVaultUri"></param>
    /// <param name="credential"></param>
    /// <param name="azureKeyVaultOptions"></param>
    /// <param name="failureStatus"></param>
    /// <param name="tags"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddAzureKeyVaultSettings(this IHealthChecksBuilder builder,
                                                                    string name,
                                                                    Uri keyVaultUri, 
                                                                    TokenCredential credential, 
                                                                    AzureKeyVaultHealthCheckOptions? azureKeyVaultOptions,
                                                                    HealthStatus failureStatus,IEnumerable<string> tags = default)
    {
        //Application settings exist
        //
        return builder.Add(new HealthCheckRegistration($"{name}",
                                                       new AzureKeyVaultHealthCheck(keyVaultUri, credential,azureKeyVaultOptions),
                                                       failureStatus,
                                                       tags));
    }
}
