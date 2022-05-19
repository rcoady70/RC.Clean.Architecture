using System.Security.Claims;
using Microsoft.Extensions.Primitives;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Infrastructure.Logging.Constants;
using RC.CA.SharedKernel.Constants;
using RC.CA.SharedKernel.WebHelpers;

namespace NT.CA.WebUiMvc.Middleware.Jwt;

public class AppContextJwtMiddleware 
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AppContextJwtMiddleware> _logger;
    //https://jasonwatmore.com/post/2021/06/15/net-5-api-jwt-authentication-with-refresh-tokens#jwt-middleware-cs
    public AppContextJwtMiddleware(RequestDelegate next, ILogger<AppContextJwtMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    /// <summary>
    /// Hydrate key claims information Userid
    ///         Validate jwt token. 
    ///         Note use action injection for DI components. 
    ///         Middleware is invoked as singleton so cannot use constructor injection on scoped or transient scoped objects.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userService"></param>
    /// <param name="jwtUtilities"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context,IAppContextX appContextX, IConfiguration configuration)
    {
        _logger.LogDebug(LoggerEvents.DebugLifeCycle, $"Executing middlware {nameof(AppContextJwtMiddleware)} Url {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request)}");
        appContextX.CdnEndpoint = configuration.GetSection("BlobStorage:ContainerName").Value;
        appContextX.ApiEndpoint = configuration.GetSection("ApiEndpoint").Value;
        appContextX.IpAddress = WebHelperRequests.GetIpAddress(context);
        //Hydrate application context
        //
        if (context != null)
        {
            var claimsIdentity = (ClaimsIdentity)context.User.Identity;
            if (claimsIdentity != null)
            {
                appContextX.IsAuthenticated = claimsIdentity.IsAuthenticated;
                var userNameClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (userNameClaim != null)
                    appContextX.UserName = !string.IsNullOrEmpty(userNameClaim.Value) ? userNameClaim.Value : "";

                var userIdClaim = claimsIdentity.FindFirst(SessionConstants.ClaimUserId);
                if (userIdClaim != null)
                    appContextX.UserId = !string.IsNullOrEmpty(userIdClaim.Value) ? userIdClaim.Value : "";

                var jwtToken = claimsIdentity.FindFirst("JwtToken");
                if (jwtToken != null)
                    appContextX.JwtToken = jwtToken.Value;

                //Get user id
                appContextX.UserId = claimsIdentity.Claims.Where(c => c.Type == SessionConstants.ClaimUserId).Select(c => c.Value).SingleOrDefault();

                //Get CorrelationId created in the EnrichLoggingMiddleware middleware
                if (context.Request.Headers.TryGetValue(WebConstants.CorrelationId, out StringValues correlationIds))
                    appContextX.CorrelationId = correlationIds;
            }
        }
        context.Response.OnStarting(() =>
        {
            return Task.CompletedTask;
        });
        await _next(context);
    }
}

