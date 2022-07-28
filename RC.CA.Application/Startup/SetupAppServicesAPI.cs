using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NT.CA.Notification.WebApi.Startup
{
    /// <summary>
    /// Setup application services register auto mapper and mediatr
    /// </summary>
    public static class SetupAppServicesAPI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Auto mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //Mediatr
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //Register mediatr validation behavior to mediatr pipeline TEST
            //
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            return services;
        }
    }
}
