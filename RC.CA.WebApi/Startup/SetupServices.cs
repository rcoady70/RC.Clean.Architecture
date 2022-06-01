using Azure.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Contracts.Services;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Application.MsgBusHandlers;
using RC.CA.Application.Services;
using RC.CA.Infrastructure.MessageBus;
using RC.CA.Infrastructure.MessageBus.Interfaces;
using RC.CA.Infrastructure.Persistence.AzureBlob;
using RC.CA.SharedKernel.WebHelpers;

namespace RC.CA.WebApi.Startup
{
    public static class SetupServices
    {
        /// <summary>
        /// Register base services required for application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBaseServicesApi(this IServiceCollection services, IConfiguration configuration)
        {
            //Application context hydrate key info from claims for easy access
            services.AddScoped<IAppContextX, AppContextX>();

            //Get JWT token settings
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            
            //Get blob storage settings
            services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));

            //Get CORS settings
            services.Configure<CorsSettings>(configuration.GetSection("CorsSettings"));

            //Http helper used to generate security nouce
            services.AddScoped<INonce, Nonce>();

            //Message bus interface
            services.Configure<MessageBusSettings>(configuration.GetSection("MessageBus"));

            //Manage blob storage
            services.AddScoped<IBlobStorage, AzureStorage>();

            //Manage csv mappings
            services.AddScoped<ICsvMapService, CsvMapService>();

            return services;
        }
        /// <summary>
        /// Register base API policy
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsPolicyApi(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
            //services.AddCors(options => options.AddPolicy("AllowEverything", builder =>
            //                                                                 builder.AllowAnyOrigin()
            //                                                                .AllowAnyMethod()
            //                                                                .AllowAnyHeader()
            //                                              )
            //);

            services.AddCors(options => options.AddPolicy(WebConstants.CORSPolicyName,
                                                          builder => {
                                                          //builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                                                              builder.WithOrigins(corsSettings.AllowedOrgins.ToArray())
                                                              .AllowAnyMethod().AllowAnyHeader()
                                                              .SetIsOriginAllowedToAllowWildcardSubdomains();
                                                          //.AllowCredentials(); //Allows cookies to be posted on ajax querys

                                                          //builder.SetPreflightMaxAge(TimeSpan.FromMinutes(10));//Chrome is hardwired to 10 
                                                          //builder.WithExposedHeaders("PageNo","PageSize","PageCount","PageTotalRecords"); //Expose headers
                                                          //builder.SetIsOriginAllowed(IsOrginAllowed); //Call custom function to check if origin is allowed
                                                          }));
            //services.AddCors(options => options.AddPolicy(WebConstants.CORSPublicPolicyName, builder =>
            //                                                           builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
            //                                                           //.WithMethods("POST")
            //                                                           //.WithHeaders("Content-Type")
            //                                             )
            //);

            return services;
        }
        /// <summary>
        /// Register base CORS policy
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsPolicyMVC(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = configuration.GetSection("CorsSettings").Get<CorsSettings>();

            services.AddCors(options => options.AddPolicy(WebConstants.CORSPolicyName,
                                                            builder => {
                                                                builder.WithOrigins(corsSettings.AllowedOrgins.ToArray())
                                                                       .SetIsOriginAllowedToAllowWildcardSubdomains() //Allow sub domains
                                                                       .AllowAnyMethod().AllowAnyHeader();
                                                                builder.SetIsOriginAllowed(IsOrginAllowed); //Call custom function to check if origin is allowed
                                                            })
                                                          );

            return services;
        }
        /// <summary>
        /// Check if cors origin at runtime. 
        /// </summary>
        /// <param name="Host"></param>
        /// <returns></returns>
        private static bool IsOrginAllowed(string Host)
        {
            //Add custom cors code
            return true;
        }
        /// <summary>
        /// Add health checks
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSiteHealthChecks(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck($"Self", () => HealthCheckResult.Healthy(), new string[] { $"{webHostEnvironment.EnvironmentName}" });

            hcBuilder.CheckConfigurationFileSettings("Check application configuration",configuration, HealthStatus.Unhealthy, tags: new[] { "Config" })
                     .AddSqlServer(configuration.GetConnectionString("Default"), tags: new[] { "Database" })
                     .AddAzureKeyVaultSettings("KeyVaultCheck",
                                              new Uri(configuration.GetValue<string>("KeyVault:VaultName")),
                                              new DefaultAzureCredential(),
                                              new NT.CA.WebUiApi.HealthChecks.AzureKeyVaultHealthCheckOptions()
                                              {
                                                  IsProduction = webHostEnvironment.IsProduction(),
                                                  Secrets = new List<string>() { "ConnectionStrings--Default", "JwtSettings--Key" }
                                              },
                                              HealthStatus.Unhealthy,
                                              tags: new[] { "Azure" });

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
                IServiceScopeFactory scopefactory = sp.GetService<IServiceScopeFactory>();
                //Creates one topic, but this can have many subscriptions with each one being registered using the subscribe method.
                //Each subscription has a filter added so the messages for each topic are separated by handler.
                // eventBus.Subscribe<SendEmailMessage, SendEMailEventHandler>();
                //
                return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                                              eventBusSubcriptionsManager,
                                              scopefactory,
                                              WebConstants.EventBusTopic,
                                              WebConstants.EventBusSubscription);
            });

            services.AddTransient<ProcessCsvImportRequestMessageHandler>();

            //See ConfigureEventBusHandlers in SetupApplication.cs, this is where event handlers are wired up to process messages...

            return services;
        }
    }
}
