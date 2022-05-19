using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using RC.CA.Application.Contracts.Identity;
using RC.CA.SharedKernel.Constants;
using RC.CA.Infrastructure.Logging.Constants;
using RC.CA.SharedKernel.WebHelpers;

namespace RC.CA.WebApi.Filters;
/// <summary>
/// Application context used to pass to the application layer
/// </summary>
public class AppContextFilter : IAsyncActionFilter
{

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //Di not supported in filters
        //
        var logger = context.HttpContext.RequestServices.GetService<ILogger<AppContextFilter>>();
        var appContextX = context.HttpContext.RequestServices.GetService<IAppContextX>();
        var nouce = context.HttpContext.RequestServices.GetService<INonce>();

        //Get CorrelationId created in the EnrichLoggingMiddleware middleware
        if (context.HttpContext.Response.Headers.TryGetValue(WebConstants.CorrelationId, out StringValues correlationIds))
            appContextX.CorrelationId = correlationIds;
        else
        {
            // If correlationId not found generate one and add to header collection
            appContextX.CorrelationId = nouce.CorrelationId;
            context.HttpContext.Request.Headers.TryAdd(WebConstants.CorrelationId, appContextX.CorrelationId);
        }
        appContextX.IpAddress = WebHelperRequests.GetIpAddress(context.HttpContext);

        logger.LogDebug(LoggerEvents.DebugLifeCycle, $"Executing API filter {nameof(AppContextFilter)} CID {appContextX.CorrelationId} Url {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.HttpContext.Request)}");
        //https://codereview.stackexchange.com/questions/200973/asp-net-core-get-user-at-service-layer
        if (context.HttpContext != null)
        {
            var claimsIdentity = (ClaimsIdentity)context.HttpContext.User.Identity;
            if (claimsIdentity != null && appContextX !=null)
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

            }
        }
        await next();
    }
}
