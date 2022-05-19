using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NT.CA.Notification.WebApi.MsgBusHandlers;
using RC.CA.Infrastructure.MessageBus.Interfaces;

namespace NT.CA.Notification.WebApi.Startup
{
    public static class SetupApplication
    {
        public static IApplicationBuilder ConfigureEventBusHandlers(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            //Setup send notification message handler
            //
            eventBus.Subscribe<SendEmailRequestMessage, SendEmailRequestHandler>();
            return app;
        }

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
}
