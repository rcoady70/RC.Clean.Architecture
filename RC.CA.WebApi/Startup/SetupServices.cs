using Azure.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Dto.Authentication;
using RC.CA.SharedKernel.Constants;
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

            //Get CORS settings
            //
            services.Configure<CorsSettings>(configuration.GetSection("CorsSettings"));

            //Http helper used to generate security nouce
            //
            services.AddScoped<INonce, Nonce>();

            //Message bus interface
            //
            services.Configure<MessageBusSettings>(configuration.GetSection("MessageBus"));

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
        public static IServiceCollection AddSiteHealthChecks(this IServiceCollection services, IConfiguration configuration,bool isProduction)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("Self", () => HealthCheckResult.Healthy());

            hcBuilder.CheckConfigurationFileSettings("Check application configuration",configuration, HealthStatus.Unhealthy, tags: new[] { "Config" })
                     .AddSqlServer(configuration.GetConnectionString("Default"), tags: new[] { "Database" })
                     .AddAzureKeyVaultSettings("KeyVaultCheck",
                                              new Uri(configuration.GetValue<string>("KeyVault:VaultName")),
                                              new DefaultAzureCredential(),
                                              new NT.CA.WebUiApi.HealthChecks.AzureKeyVaultHealthCheckOptions()
                                              {
                                                  IsProduction = isProduction,
                                                  Secrets = new List<string>() { "ConnectionStrings--Default", "JwtSettings--Key" }
                                              },
                                              HealthStatus.Unhealthy,
                                              tags: new[] { "Azure" });

            return services;
        }
    }
}
