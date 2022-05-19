using Microsoft.Extensions.Diagnostics.HealthChecks;
using NT.CA.Notification.WebApi.MsgBusHandlers;
using NT.CA.Notification.WebApi.Services;
using RC.CA.Infrastructure.MessageBus;
using RC.CA.Infrastructure.MessageBus.Interfaces;
using RC.CA.SharedKernel.Constants;
using RC.CA.SharedKernel.GuardClauses;

namespace NT.CA.Notification.WebApi.Startup
{
    public static class StartupServices
    {
        /// <summary>
        /// Add health checks
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSiteHealthChecks(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck($"Self {webHostEnvironment.EnvironmentName}", () => HealthCheckResult.Healthy());

            hcBuilder.AddAzureServiceBusTopic(
                       configuration["EventBus:ConnectionString"],
                       topicName: "rc.ca.webapi.topic",
                       name: "emailnotification-servicebus-check",
                       tags: new string[] { "servicebus" });

            return services;
        }

        /// <summary>
        /// Add email provider
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEmailProvider(this IServiceCollection services, IConfiguration configuration)
        {
            //Get email provider settings
            services.Configure<EmailProviderSettings>(configuration.GetSection("EmailProvider"));
            services.AddSingleton<IEmailProvider, NT.CA.Notification.WebApi.Services.EmailProviderSendGrid>();
            return services;
        }

        /// <summary>
        /// Add event bus
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            //Add event message and handler 
            services.AddSingleton<IServiceBusSubscriptionsManager, ServiceBusSubscriptionsManager>();

            //Add event bus connection
            services.AddSingleton<IServiceBusConnection>(sp =>
            {
                var serviceBusConnectionString = configuration["EventBus:ConnectionString"];
                Guard.Against.Null(serviceBusConnectionString, nameof(serviceBusConnectionString));
                return new ServiceBusConnection(serviceBusConnectionString);
            });

            //Initialize event bus
            services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
            {
                //Get instances of dependent objects from DI container to ensure they container can correctly dispose of them
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IServiceBusSubscriptionsManager>();

                //Creates one topic, but this can have many subscriptions with each one being registered using the subscribe method.
                //Each subscription has a filter added so the messages for each topic are separated by handler.
                // eventBus.Subscribe<SendEmailMessage, SendEMailEventHandler>();
                //
                return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                                              eventBusSubcriptionsManager,
                                              sp,
                                              WebConstants.EventBusTopic,
                                              WebConstants.EventBusSubscription);
            });

            services.AddTransient<SendEmailRequestHandler>();

            //See ConfigureEventBusHandlers in SetupApplication.cs, this is where event handlers are wired up to process messages...

            return services;
        }
    }
}
