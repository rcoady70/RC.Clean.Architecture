using Microsoft.Extensions.Primitives;
using RC.CA.SharedKernel.Constants;
using Serilog.Context;
using RC.CA.Infrastructure.Logging.Constants;
using RC.CA.SharedKernel.WebHelpers;

namespace RC.CA.WebUiMvc.Middleware.Logging;

public class EnrichLoggingMiddlewareMVC
{
    private readonly RequestDelegate _next;
    private readonly ILogger<EnrichLoggingMiddlewareMVC> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    public EnrichLoggingMiddlewareMVC(RequestDelegate next, ILogger<EnrichLoggingMiddlewareMVC> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// Enrich log entries. Note use action injection for DI components. 
    ///                     Middleware is invoked as singleton so cannot use constructor injection on scoped or transient scoped objects.
    /// <param name="context"></param>
    /// <param name="appContext"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, INonce nouce)
    {
        _logger.LogDebug(LoggerEvents.DebugLifeCycle, $"Executing MVC middleware {nameof(EnrichLoggingMiddlewareMVC)} CID {nouce.CorrelationId} Url {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request)}");
        context.Response.Headers.TryGetValue(WebConstants.CorrelationId, out StringValues correlationId);
        //Enrich log information
        //
        EnrichLog(context, correlationId);

        context.Response.OnStarting(() =>
        {

            return Task.CompletedTask;
        });
        await _next.Invoke(context);
    }

    /// <summary>
    /// Enrich log info
    /// </summary>
    /// <param name="context"></param>
    private void EnrichLog(HttpContext context, string correlationId)
    {
        LogContext.PushProperty($"AppCtx-UserAgent", context.Request.Headers["User-Agent"]);
        LogContext.PushProperty($"AppCtx-{WebConstants.CorrelationId}", correlationId ?? "");
        LogContext.PushProperty("AppCtx-IsAuthenticated", context.User?.Identity?.IsAuthenticated);
        LogContext.PushProperty("AppCtx-UserName", context.User?.Identity?.Name);
        LogContext.PushProperty("AppCtx-IpAddress", WebHelperRequests.GetIpAddress(context));
        LogContext.PushProperty("AppCtx-Host", Environment.MachineName);
        LogContext.PushProperty("AppCtx-ProcessId", Environment.ProcessId);
        LogContext.PushProperty("AppCtx-Url", Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request));

    }
}
