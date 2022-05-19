using RC.CA.Application.Contracts.Identity;
using NT.CA.WebUiMvc.Middleware.Jwt;
using RC.CA.WebUiMvc.Middleware.Logging;

namespace RC.CA.WebUiMvc.Middleware.SecurityHeaders;

public static class MiddlewareExtensions
{
    //
    // Add security headers middle ware
    //
    public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app, SecurityHeadersBuilder builder)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        return app.UseMiddleware<SecurityHeadersMiddleware>(builder.Build());
    }
    //
    // [Serilog] Log enrichment. 
    //
    public static IApplicationBuilder UseLogEnrichmentMiddleware(this IApplicationBuilder app)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));
        return app.UseMiddleware<EnrichLoggingMiddlewareMVC>();
    }
    //
    // [Authentication] Hydrate application context from claims IAppContextx and validate jwt token
    //
    public static IApplicationBuilder UseAppContextMiddleware(this IApplicationBuilder app)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        return app.UseMiddleware<AppContextJwtMiddleware>();
    }
}

