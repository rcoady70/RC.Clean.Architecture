using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RC.CA.Application.Startup;


public static class SetupAppServicesMvc
{
    public static IServiceCollection AddApplicationServicesMVC(this IServiceCollection services, IConfiguration configuration)
    {
        //Auto mapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
}

