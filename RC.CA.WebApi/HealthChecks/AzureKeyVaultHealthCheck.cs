
using System.Collections.Concurrent;
using Azure.Core;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RC.CA.SharedKernel.GuardClauses;

namespace NT.CA.WebUiApi.HealthChecks;

public class AzureKeyVaultHealthCheck : IHealthCheck
{
    private readonly Uri _keyVaultUri;
    private readonly TokenCredential _credential;
    private readonly AzureKeyVaultHealthCheckOptions _azureKeyVaultOptions;
    private IReadOnlyDictionary<string, object> _healthCheckData;

    /// <summary>
    /// Check access to azure key vault with list of secrets which should exist
    /// </summary>
    /// <param name="keyVaultUri"></param>
    /// <param name="credential"></param>
    /// <param name="keys"></param>
    public AzureKeyVaultHealthCheck(Uri keyVaultUri, TokenCredential credential, AzureKeyVaultHealthCheckOptions? azureKeyVaultOptions)
    {
        Guard.Against.Null(keyVaultUri,nameof(keyVaultUri));
        Guard.Against.Null(credential, nameof(credential));
        Guard.Against.Null(azureKeyVaultOptions, nameof(azureKeyVaultOptions));

        _keyVaultUri = keyVaultUri;
        _credential = credential;
        _azureKeyVaultOptions = azureKeyVaultOptions;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = new SecretClient(_keyVaultUri, _credential);
            foreach (var key in _azureKeyVaultOptions.Secrets)
            {
                var secret = await client.GetSecretAsync(key);
                if (secret == null)
                    return new HealthCheckResult(HealthStatus.Degraded, $"Azure key vault secret not found {key}. IsProduction {_azureKeyVaultOptions.IsProduction}");
                if(secret.Value.Value.Length  == 0)
                    return new HealthCheckResult(HealthStatus.Degraded, $"Azure key vault secret found value is blank {key}. IsProduction {_azureKeyVaultOptions.IsProduction}");
            }

            return new HealthCheckResult(HealthStatus.Healthy, $"Azure key vault access verified all secrets found. IsProduction {_azureKeyVaultOptions.IsProduction}");
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(HealthStatus.Unhealthy,$"Azure key vault check failed exception. IsProduction {_azureKeyVaultOptions.IsProduction}", exception: ex);
        }
    }

}


