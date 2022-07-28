using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace RC.CA.WebApi.Tests.Xunit.Setup;

public class WebApiApplication : WebApplicationFactory<Program>
{
    private readonly ITestOutputHelper _testOutputHelper;

    public WebApiApplication(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    /// <summary>
    /// Create host
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    protected override IHost CreateHost(IHostBuilder builder)
    {

        builder.UseEnvironment("Testing");

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new XUnitLoggerProvider(_testOutputHelper));
        });

        builder.ConfigureServices(services =>
        {
            ////Remove dbcontext for di container
            //var descriptor = services.SingleOrDefault(
            //        d => d.ServiceType ==
            //                 typeof(DbContextOptions<ApplicationDbContext>));
            //services.Remove(descriptor);

            ////Add dbcontext for di container
            //string dbName = "InMemoryDbForTesting" + Guid.NewGuid().ToString();
            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseInMemoryDatabase(dbName);
            //});


            var serviceProvider = services.BuildServiceProvider();
        });
        return base.CreateHost(builder);
    }
}
