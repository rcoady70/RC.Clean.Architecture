using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RC.CA.Application.Behaviours;

namespace NT.CA.Notification.WebApi.Startup
{
    /// <summary>
    /// Setup application services register auto mapper and mediatr
    /// </summary>
    public static class SetupAppServicesAPI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //[Automapper] Register auto mapper methods
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //[Mediatr]
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //[Mediatr]  Validation behaviour
            //
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatrRequestValidationBehavior<,>));

            //[Mediatr] Add caching 
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddDistributedMemoryCache();
            return services;
        }
    }
}
