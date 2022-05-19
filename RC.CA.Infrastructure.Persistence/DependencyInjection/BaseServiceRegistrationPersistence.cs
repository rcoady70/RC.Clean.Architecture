using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Infrastructure.Persistence.AuthorizationJwt;
using RC.CA.Infrastructure.Persistence.Services;

namespace RC.CA.Infrastructure.Persistence.DependencyInjection;

public static class BaseServiceRegistrationPersistence
{
    /// <summary>
    /// Register base services required for application
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBaseServicesPersistence(this IServiceCollection services)
    {
       
        //Jwt helper functions
        services.TryAddScoped<IJwtUtilities, JwtUtilities>();

        //Authentication services
        services.AddTransient<IAuthService, AuthService>();

        return services;
    }
}
