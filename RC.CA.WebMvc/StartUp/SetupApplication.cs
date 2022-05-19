using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RC.CA.Infrastructure.Logging;
using RC.CA.Infrastructure.Logging.Middleware.Exceptions;

namespace RC.CA.WebMvc.StartUp;

public static class SetupApplication
{
    public static IEndpointRouteBuilder ConfigureHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        //[Healthchecks]
        endpoints.MapHealthChecks("/health/services", new HealthCheckOptions()
        {
            //run check to test downstream services.
            Predicate = s => s.Tags.Contains("service"),
            ResultStatusCodes ={
                                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                                    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                                },
            //format json response for healthcheck
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).AllowAnonymous();
        endpoints.MapHealthChecks("/health", new HealthCheckOptions()
        {
            //run all checks. Checks can be run by tag just use predicate to select tags to run.
            Predicate = _ => true,
            ResultStatusCodes =
                                {
                                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                                    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                                },
            //format json response for healthcheck
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).AllowAnonymous();
        return endpoints;
    }
}
