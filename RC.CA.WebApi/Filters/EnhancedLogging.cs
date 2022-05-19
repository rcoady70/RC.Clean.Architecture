using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using RC.CA.Infrastructure.Logging.Constants;

namespace RC.CA.WebApi.Filters;

/// <summary>
/// Enhanced loggin. Log slow executing actions
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class EnhancedLogging : ActionFilterAttribute
{
    private Stopwatch _timer;
    private readonly ILogger<EnhancedLogging> _logger;
    public EnhancedLogging(ILogger<EnhancedLogging> logger)
    {
        _logger = logger;
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _timer = new Stopwatch();
        _timer.Start();
    }
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        //Log performance if it takes more than 4 seconds for controller to run
        //
        _timer.Stop();
        // Do not log performance if there was an exception may cause skew.
        if (context.Exception == null && _timer.ElapsedMilliseconds >= 4000)
        {
            _logger.LogWarning(LoggerEvents.PerformanceEvt, @"Performance route: {RouteName} method: {Method} took {ElapsedMilliseconds} seconds ", context.HttpContext.Request.Path, context.HttpContext.Request.Method, _timer.ElapsedMilliseconds);
        }
    }
}
