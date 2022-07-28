using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NT.CA.Notification.WebApi.Startup;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Infrastructure.Persistence;
using Respawn;

namespace RC.CA.WebApi.Tests.Nunit;

//Global setup and clean up
//
[SetUpFixture]
public class TestingHelper
{
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    private static Checkpoint _checkpoint;
    private static string _currentUserId;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {

        //Load configuration 
        //
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        // Build service collection
        //
        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "RC.CA.WebApi.Test"));

        //Add application context
        var appcontextX = Mock.Of<IAppContextX>();
        appcontextX.UserName = "Unit Test";
        appcontextX.UserId = "9999";
        appcontextX.IsAuthenticated = true;
        appcontextX.CspNouce = "";
        appcontextX.CorrelationId = "";
        appcontextX.CdnEndpoint = "";
        appcontextX.ApiEndpoint = "";
        services.AddSingleton<IAppContextX>(appcontextX);

        //Setup MediatR and Automapper
        services.AddApplicationServices(_configuration);

        services.SetupDbContextServices(_configuration.GetConnectionString("Default"), true);

        //// Replace service registration for ICurrentUserService
        //// Remove existing registration
        //var currentUserServiceDescriptor = services.FirstOrDefault(d =>
        //    d.ServiceType == typeof(ICurrentUserService));

        //services.Remove(currentUserServiceDescriptor);

        //// Register testing version
        //services.AddScoped(provider =>
        //    Mock.Of<ICurrentUserService>(s => s.UserId == _currentUserId));
        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

        //Ignore migrations table
        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
        };

        //EnsureDatabase();
    }

    //private void EnsureDatabase()
    //{
    //    using var scope = _scopeFactory.CreateScope();

    //    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

    //    context.Database.Migrate();
    //}
    /// <summary>
    /// Reset database state
    /// </summary>
    /// <returns></returns>
    public static async Task ResetState()
    {
        try
        {
            await _checkpoint.Reset(_configuration.GetConnectionString("Default"));
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }
        _currentUserId = null;
    }

    //public static async Task<TEntity> FindAsync<TEntity>(int id)
    //    where TEntity : class
    //{
    //    using var scope = _scopeFactory.CreateScope();

    //    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

    //    return await context.FindAsync<TEntity>(id);
    //}

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        try
        {


            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    //public static async Task<string> RunAsDefaultUserAsync()
    //{
    //    var userName = "test@local";
    //    var password = "Testing1234!";

    //    using var scope = _scopeFactory.CreateScope();

    //    var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

    //    var user = new ApplicationUser { UserName = userName, Email = userName };

    //    var result = await userManager.CreateAsync(user, password);

    //    _currentUserId = user.Id;

    //    return _currentUserId;
    //}
}

