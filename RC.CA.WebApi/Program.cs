using RC.CA.WebApi;
using RC.CA.Infrastructure.Persistence;
using RC.CA.WebApi.Filters;
using Serilog;
using RC.CA.Infrastructure.Persistence.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Azure;
using Azure.Identity;
using RC.CA.WebApi.Startup;
using NT.CA.Notification.WebApi.Startup;


// [Serilog] Setup serilog in a two-step process. First, we configure basic logging
// to be able to log errors during ASP.NET Core startup. Later, we read
// log settings from appsettings.json. Read more at
// https://github.com/serilog/serilog-aspnetcore#two-stage-initialization.
// General information about serilog can be found at
// https://serilog.net/
Log.Logger = new LoggerConfiguration()
            .Enrich.With()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
try
 {
    //[Serilog]
    Log.Information($"{DateTime.Now.ToString()} Web api starting Environment.Version {Environment.Version}");
    var builder = WebApplication.CreateBuilder(args);

    //[KeyVault] If production hook up key vault
    //
    if (builder.Environment.IsProduction())
        builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration.GetValue<string>("KeyVault:VaultName")), new DefaultAzureCredential());

    //[Serilog] full setup take settings from application settings
    //
    builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
                                                                               .ReadFrom.Services(services)
                                                                               .Enrich.FromLogContext());

    //[ApplicationDbContext] setup application context
    //
    builder.Services.SetupDbContextServices(builder.Configuration.GetConnectionString("Default"), builder.Environment.IsDevelopment());

    //[Dependency Injection][Identity Authorization] setup identity. Setup default user and role 
    //
    builder.Services.AddIdentityServiceServicesApi(builder.Configuration);
    builder.Services.AddJwtAuthenticationApi(builder.Configuration);
    builder.Services.AddBaseServicesPersistence(); //JwtUtilities and AuthService

    //[DependencyInjection] Register application services. ,IAppContextX,JwtSettings,CorsSettings
    //
    builder.Services.AddBaseServicesApi(builder.Configuration);
    builder.Services.AddApplicationServices(builder.Configuration); //AddAutoMapper, AddMediatR

    //[CORS]
    //
    builder.Services.AddCorsPolicyApi(builder.Configuration);

    // Add controller services to the container.
    //
    builder.Services.AddControllers()
                    .AddFluentValidation();

    //[EventBus] Notification requests will be queued to azure event buss and processed later
    //
    builder.Services.AddEventBus(builder.Configuration);

    //[Swagger] Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    //[Swagger] add jwt identity tokens
    builder.Services.SetupSwaggerServices();

    //[Healthcheck] Check database is available
    builder.Services.AddSiteHealthChecks(builder.Configuration,builder.Environment);

    //[Filters] Add MVC filters
    builder.Services.AddControllers(config =>
    {
        //Extract application context user info from claims for dependency injection
        config.Filters.Add(new AppContextFilter());
        //[SeriLog] Performance logging of slow running action
        config.Filters.Add(typeof(EnhancedLogging));
    });

    //[GlobalAuthorization] Authorization is on an opt out basis. Opt out of using [AllowAnonymous] data annotation
    builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
    });

    //Azure blob storage to save images
    var blobConnection = builder.Configuration.GetSection("BlobStorage:ConnectionString").Value;
    builder.Services.AddAzureClients(builder =>
    {
        builder.AddBlobServiceClient(blobConnection);
    });

    //
    //[Info] Middleware order execution
    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#middleware-order
    //
    var app = builder.Build();

    //[EventBus] Configure azure bus message handlers
    app.ConfigureEventBusHandlers();

    //[SeriLog] Serilog Enrich log entries...
    //
    RC.CA.WebApi.Middleware.MiddlewareExtensions.UseLogEnrichmentMiddleware(app);

    //[Exceptions] Global error handling.
    //
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        //[Exceptions] Global exception handler implemented as middleware
        app.UseApiExceptionHandler(options =>
        {
            //Change log level from error to fatal depending on the error type
            options.DetermineLogLevel = DetermineLogLevel;
        });
    }

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
   // {
        app.UseSwagger();
        app.UseSwaggerUI();
    // }

    //Execute order 0. Exception handler 1. HSTS 2. HttpsRedirection 3. Static files 
    //              4. Routing 5. CORS 6. Authentication 7. Authorization 8. Custom middleware

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging(opts =>
    {
        opts.EnrichDiagnosticContext = (diagCtx, httpCtx) =>
        {
            diagCtx.Set("xMachine", Environment.MachineName);
            diagCtx.Set("xClientIP", httpCtx.Connection.RemoteIpAddress);
            diagCtx.Set("xUserAgent", httpCtx.Request.Headers["User-Agent"]);
            if (httpCtx.User.Identity?.IsAuthenticated == true)
            {
                diagCtx.Set("xUserName", httpCtx.User.Identity?.Name);
            }
        };
    });

    //[CORS] Order is important in pipeline.
    app.UseCors(RC.CA.SharedKernel.Constants.WebConstants.CORSPolicyName);
    //app.UseCors("PublicApi");

    //[Identity] Enable authentication
    app.UseAuthentication();

    app.UseAuthorization();
 
    app.MapControllers();

    //[Healthchecks] Configure azure bus message handlers
    app.ConfigureHealthChecks();
    
    app.Run();
 }
 catch (Exception ex)
 {
    Log.Fatal(ex, $"{DateTime.Now.ToString()} Web api terminated unexpectedly {ex.Message}");
 }
 finally
 {
    Log.CloseAndFlush();
 }


//
// Change log level depending on the type of error
//
LogLevel DetermineLogLevel(Exception ex)
{
    //Critical database errors
    if (ex.Message.Contains("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
        ex.Message.Contains("a network-related", StringComparison.InvariantCultureIgnoreCase) ||
        ex.Message.Contains("not enough space on the disk", StringComparison.InvariantCultureIgnoreCase))
    {
        return LogLevel.Critical;
    }
    return LogLevel.Error;
}


