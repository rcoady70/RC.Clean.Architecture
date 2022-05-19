using System.Reflection;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Contracts.Services;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Application.Services;
using RC.CA.SharedKernel.Constants;
using RC.CA.SharedKernel.WebHelpers;

namespace RC.CA.WebMvc.StartUp;

public static class SetupServices
{
    /// <summary>
    /// Register base services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBaseServicesUi(this IServiceCollection services, IConfiguration configuration)
    {
        //Cookie authentication (additional cookie validation to ensure cookie has a finite life span)
        services.TryAddTransient<CookieAuthenticationEvent>();

        services.TryAddSingleton<JavaScriptEncoder>();

        //Application context, hydrated using AppContextFilter on each request. Eg gives access to jwttoken, user info etc
        services.TryAddScoped<IAppContextX, AppContextX>();

        //Get JWT token settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        //Http helper used to interact with web api.
        services.TryAddScoped<IHttpHelper, HttpHelper>();

        //Http helper used to generate security nouce csp policy
        services.TryAddScoped<INonce, Nonce>();

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
    /// Check if cors origin at runtime
    /// </summary>
    /// <param name="Host"></param>
    /// <returns></returns>
    private static bool IsOrginAllowed(string Host)
    {
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

        hcBuilder.AddCheck($"Self", () => HealthCheckResult.Healthy(), new string[] { $"{webHostEnvironment.EnvironmentName}" })
                 .AddUrlGroup(new Uri($"{configuration.GetValue<string>("ApiEndpoint")}/health"), $"API ready - {new Uri($"{configuration.GetValue<string>("ApiEndpoint")}/health")}", HealthStatus.Unhealthy, tags: new[] { "api" }, new TimeSpan(0, 0, 5))
                 .AddFilePathWrite("Check image folder access", $"{Directory.GetCurrentDirectory()}\\wwwroot\\images", HealthStatus.Unhealthy, tags: new[] { "access" })
                 .AddFilePathWrite("Check log folder access", $"{Directory.GetCurrentDirectory()}\\logs", HealthStatus.Unhealthy, tags: new[] { "access" });

        return services;
    }
}
