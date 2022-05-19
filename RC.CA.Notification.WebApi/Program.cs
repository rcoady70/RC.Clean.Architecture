
using System.Reflection;
using Azure.Identity;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NT.CA.Notification.WebApi.DTO;
using NT.CA.Notification.WebApi.MsgBusHandlers;
using NT.CA.Notification.WebApi.Startup;
using RC.CA.Infrastructure.MessageBus.Interfaces;
using Serilog;

Log.Logger = new LoggerConfiguration()
            .Enrich.With()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    //[KeyVault] If production hook up key vault
    if (builder.Environment.IsProduction())
        builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration.GetValue<string>("KeyVault:VaultName")), new DefaultAzureCredential());

    //[Serilog] full setup take settings from application settings
    builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
                                                                               .ReadFrom.Services(services)
                                                                               .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddControllers()
                    .AddFluentValidation();

    //[Email] Add email provider
    //
    builder.Services.AddEmailProvider(builder.Configuration);

    //[EventBus] Notification requests will be queued to azure event buss and processed later
    //
    builder.Services.AddEventBus(builder.Configuration);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //[Healthcheck] Add site health checks
    builder.Services.AddSiteHealthChecks(builder.Configuration);

    //Auto mapper
    var serviceCollection = builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    var app = builder.Build();

    //[EventBus] Configure azure bus message handlers
    app.ConfigureEventBusHandlers();

    //[Healthchecks] Configure azure bus message handlers
    app.ConfigureHealthChecks();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
        app.UseSwagger();
        app.UseSwaggerUI();
    //}

    app.UseHttpsRedirection();

    //[Serilog] Enrich logging information
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

    app.UseAuthorization();

    app.MapControllers();

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

