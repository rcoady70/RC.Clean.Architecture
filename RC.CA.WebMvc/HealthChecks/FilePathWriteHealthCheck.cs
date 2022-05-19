using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NT.CA.WebUiMvc.HealthChecks;

public class FilePathWriteHealthCheck : IHealthCheck
{
    private string _filePath;
    private IReadOnlyDictionary<string, object> _HealthCheckData;

    public FilePathWriteHealthCheck(string filePath)
    {
        _filePath = filePath;
        _HealthCheckData = new Dictionary<string, object>
            {
                { "filePath", _filePath }
            };
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var testFile = $"{_filePath}\\FilePathWriteHealthCheck.CheckHealthAsync.txt";
            var fs = File.Create(testFile);
            fs.Close();
            File.Delete(testFile);

            return Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (Exception ex)
        {

            switch (context.Registration.FailureStatus)
            {
                case HealthStatus.Degraded:
                    return Task.FromResult(HealthCheckResult.Degraded($"Exception writing to file path",
                                                                        ex, _HealthCheckData));
                case HealthStatus.Healthy:
                    return Task.FromResult(HealthCheckResult.Healthy($"Exception writing to file path", _HealthCheckData));
                default:
                    return Task.FromResult(HealthCheckResult.Unhealthy($"Exception writing to file path",
                                                                        ex, _HealthCheckData));
            }
        }
    }
}
