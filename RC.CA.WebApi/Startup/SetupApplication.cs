﻿using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using RC.CA.Application.MsgBusHandlers;
using RC.CA.Infrastructure.Logging;
using RC.CA.Infrastructure.Logging.Middleware.Exceptions;
using RC.CA.Infrastructure.MessageBus.Interfaces;

namespace RC.CA.WebApi.Startup;

public static class SetupApplication
{
    /// <summary>
    /// Configure event bus to process import messages
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder ConfigureEventBusHandlers(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        //Setup handler to process messages from event bus 
        //
        eventBus.Subscribe<ProcessCsvImportRequestMessage, ProcessCsvImportRequestMessageHandler>();
        return app;
    }
    /// <summary>
    /// Setup error middleware
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder,
                                                             Action<ApiExceptionOptions> configureOptions)
    {
        var options = new ApiExceptionOptions();
        configureOptions(options);

        return builder.UseMiddleware<ApiExceptionMiddleware>(options);
    }
    /// <summary>
    /// Setup health checks
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder ConfigureHealthChecks(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            //format json response for healthcheck
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).AllowAnonymous();
        return app;
    }
}
