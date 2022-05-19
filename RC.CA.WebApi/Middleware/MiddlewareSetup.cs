
using RC.CA.WebApi.Middleware.Logging;

namespace RC.CA.WebApi.Middleware;
/// <summary>
/// Setup middleware extensions
/// </summary>
public static class MiddlewareExtensions
{
    //
    // [Serilog] Log enrichment. 
    //
    public static IApplicationBuilder UseLogEnrichmentMiddleware(this IApplicationBuilder app)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        return app.UseMiddleware<EnrichLoggingMiddlewareApi>();
    }
}
