using Microsoft.Extensions.Options;

namespace NT.CA.WebUiApi.HealthChecks;

    /// <summary>
    /// Azure KeyVault configuration options.
    /// </summary>
    public class AzureKeyVaultHealthCheckOptions 
    {
        public List<string> Secrets = new List<string>();
        public bool IsProduction { get; set; }

    }

