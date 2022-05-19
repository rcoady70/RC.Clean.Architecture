using RC.CA.Infrastructure.Logging.Constants;
using Microsoft.Extensions.Primitives;
using RC.CA.Application.Contracts.Identity;
using RC.CA.SharedKernel.Constants;
using RC.CA.SharedKernel.WebHelpers;
using RC.CA.WebApi.Utilities;
using Serilog.Context;

namespace RC.CA.WebApi.Middleware.Logging;
///
/// Enrich logging. Note use action injection for DI components. 
///                 Middleware is invoked as singleton so cannot use constructor injection on scoped or transient scoped objects. 
public class EnrichLoggingMiddlewareApi
{
    private readonly RequestDelegate _next;
    private readonly ILogger<EnrichLoggingMiddlewareApi> _logger;

    public EnrichLoggingMiddlewareApi(RequestDelegate next, ILogger<EnrichLoggingMiddlewareApi> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, IAppContextX appContextX,INonce nouce)
    {
        //Get CorrelationId is passed forward in request header. Used to tie back end and front end request together.
        //If no header generate a new one
        context.Request.Headers.TryGetValue(WebConstants.CorrelationId, out StringValues correlationId);
        if (string.IsNullOrEmpty(correlationId))
            correlationId = nouce.CorrelationId;

        _logger.LogDebug(LoggerEvents.DebugLifeCycle, $"Executing API middleware {nameof(EnrichLoggingMiddlewareApi)} CID {appContextX.CorrelationId} Url {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request)}");

        appContextX.CorrelationId = correlationId;
        //
        context.Response.OnStarting(() =>
        {
            return Task.CompletedTask;
        });

        //Enrich log information
        //
        EnrichLog(context, correlationId);

        await _next(context);
    }

    /// <summary>
    /// Enrich log info
    /// </summary>
    /// <param name="context"></param>
    private void EnrichLog(HttpContext context,string correlationId)
    {
        LogContext.PushProperty($"AppCtx-UserAgent", context.Request.Headers["User-Agent"]);
        LogContext.PushProperty($"AppCtx-{WebConstants.CorrelationId}", correlationId);
        LogContext.PushProperty("AppCtx-IsAuthenticated", context.User?.Identity?.IsAuthenticated);
        LogContext.PushProperty("AppCtx-UserName", context.User?.Identity?.Name);
        LogContext.PushProperty("AppCtx-IpAddress", WebHelperRequests.GetIpAddress(context));
        LogContext.PushProperty("AppCtx-Host", Environment.MachineName);
        LogContext.PushProperty("AppCtx-ProcessId", Environment.ProcessId);
        //LogContext.PushProperty($"AppCtx-JWT", WebHelperRequests.GetHeaderValue(context, "Authentication"));
    }
}
