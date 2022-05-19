using RC.CA.WebUiMvc.Middleware.SecurityHeaders;
using RC.CA.Application.Contracts.Identity;

namespace RC.CA.WebUiMvc.Middleware;

/// <summary>
/// Middleware setup
/// </summary>
public  static class SetupMiddleware
{
    public static void UseSecurityHeadersMiddleWare(WebApplication app)
    {
        //
        //Add security headers requires app settings
        //
        if (app.Configuration.GetSection("SecurityHeaders").Exists())
        {
            ISecurityHeadersDto securityHeadersDto = new SecurityHeadersDto();
            app.Configuration.GetSection("SecurityHeaders").Bind(securityHeadersDto);
            app.UseSecurityHeadersMiddleware(new RC.CA.WebUiMvc.Middleware.SecurityHeaders.SecurityHeadersBuilder().AddDefaultSecurePolicy(securityHeadersDto));
        }
    }
}
