using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Infrastructure.Persistence.Repository;

namespace RC.CA.Infrastructure.Persistence;

public static class SetupDbContextService
{



    /// <summary>
    /// Setup application context, connection string etc
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection SetupDbContextServices(this IServiceCollection services, string connectionString, bool isDevelopment)
    {
        //Create configuration as a delegate
        Action<DbContextOptionsBuilder> dbConfig = (options) =>
        {
            options.UseSqlServer(connectionString)
                   .UseLoggerFactory(CreateLoggerFactory(isDevelopment))
                   .EnableSensitiveDataLogging(isDevelopment)
                   //Set default tracking behavior to no tracking
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        };
        services.AddDbContext<ApplicationDbContext>((options) => dbConfig(options));

        // Change database connection to be encapsulated in the db context
        //services.AddDbContext<ApplicationDbContext>((options) =>
        //{
        //    options.UseSqlServer(connectionString)
        //           .UseLoggerFactory(CreateLoggerFactory(isDevelopment))
        //           .EnableSensitiveDataLogging(isDevelopment)

        //           //Set default tracking behavior to no tracking
        //           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        //});

        //Generic repository
        services.TryAddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
        services.TryAddScoped<IMemberRepository, MemberRepository>();
        services.TryAddScoped<IExperienceRepository, ExperienceRepository>();
        services.TryAddScoped<ICdnFileRepository, CdnFileRepository>();
        services.TryAddScoped<IJwtRefreshTokenRepository, JwtRefreshTokenRepository>();
        services.TryAddScoped<ICsvFileRepository, CsvFileRepository>();

        return services;
    }
    /// <summary>
    /// Logging options for db context
    /// </summary>
    /// <returns></returns>
    private static ILoggerFactory CreateLoggerFactory(bool isDevelopment)
    {
        if (isDevelopment)
            return LoggerFactory.Create(builder => builder
                                .AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                                .AddConsole());
        else
            return LoggerFactory.Create(builder => builder.AddFilter((category, level) => false));
    }
}
