using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Infrastructure.Logging.Constants;
using RC.CA.SharedKernel.Constants;
using RC.CA.SharedKernel.WebHelpers;
using RC.CA.WebUiMvc.Middleware.SecurityHeaders;

namespace RC.CA.WebUiMvc.Middleware.SecurityHeaders;

/// <summary>
/// An ASP.NET middleware for adding security headers.
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISecurityHeadersDto _securityHeaders;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;

    /// <summary>
    /// Instantiates a new <see cref="SecurityHeadersMiddleware"/>.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="securityHeaders">An instance of the security headers from app.config.</param>
    public SecurityHeadersMiddleware(RequestDelegate next, ISecurityHeadersDto securityHeaders,ILogger<SecurityHeadersMiddleware> logger)
    {
        _next = next;
        _securityHeaders = securityHeaders;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, INonce nouce, IAppContextX appContextX)
    {
        appContextX.CspNouce = nouce.CspNonce;
        appContextX.CorrelationId = nouce.CorrelationId;

        _logger.LogDebug(LoggerEvents.DebugLifeCycle, $"Executing MVC middleware {nameof(SecurityHeadersMiddleware)} Nonce {appContextX.CspNouce} CorrolationId {appContextX.CorrelationId} Url {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request)}");
        // Usefull links https://report-uri.com/home/generate  generate policy on line 
        // Check headers https://securityheaders.com/

        // Content security policy
        // Consider require-sri-for on script and style tags. Forces hash onto links. example "require-sri-for script style"  
        //
        if (!string.IsNullOrEmpty(_securityHeaders.Csp))
        {

            context.Response.Headers.TryAdd("Content-Security-Policy", $"{_securityHeaders.Csp.Replace("{nonce}", $"{appContextX.CspNouce}")} {_securityHeaders.CspReportUri}");
            context.Response.Headers.TryAdd("X-XSS-Protection", $"1; mode=block"); //older browsers 
        }
        if (!string.IsNullOrEmpty(_securityHeaders.CspReportOnly))
            context.Response.Headers.TryAdd("Content-Security-Policy-Report-Only", $"{_securityHeaders.CspReportOnly.Replace("{nonce}", $"{appContextX.CspNouce}")} {_securityHeaders.CspReportUri}");

        //Old school frame blocking
        //
        if (!string.IsNullOrEmpty(_securityHeaders.XFrameOptions))
            context.Response.Headers.TryAdd("X-Frame-Options", $"{_securityHeaders.XFrameOptions}");
        
        //Turn off features of the browser 
        //
        if (!string.IsNullOrEmpty(_securityHeaders.FeaturePolicy))
            context.Response.Headers.TryAdd("Feature-Policy", $"{_securityHeaders.FeaturePolicy}");
        
        //Ensure mime type is set correctly. If mime type not set images for example may not display
        if (!string.IsNullOrEmpty(_securityHeaders.XContentTypeOptions))
            context.Response.Headers.TryAdd("X-Content-Type-Options", $"{_securityHeaders.XContentTypeOptions}");
        
        //Set url referrer policy
        //
        if (!string.IsNullOrEmpty(_securityHeaders.ReferrerPolicy))
            context.Response.Headers.TryAdd("Referrer-Policy", $"{_securityHeaders.ReferrerPolicy}");
        
        //Set strict transport policy
        //
        if (!string.IsNullOrEmpty(_securityHeaders.StrictTransportSecurity))
            context.Response.Headers.TryAdd("Strict-Transport-Security", $"{_securityHeaders.StrictTransportSecurity}");
        
        // Set cache control
        //
        if (!string.IsNullOrEmpty(_securityHeaders.CacheControl))
            context.Response.Headers.TryAdd("Cache-Control", $"{_securityHeaders.CacheControl}");

        //Add CorrelationId header to tie back and front end requests
        //
        context.Response.Headers.TryAdd(WebConstants.CorrelationId, appContextX.CorrelationId);
        await _next(context);
    }
}
