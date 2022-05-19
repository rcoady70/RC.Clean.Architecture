using Microsoft.AspNetCore.Mvc.Formatters;
using RC.CA.WebUiMvc.Middleware.SecurityHeaders;
using RC.CA.Infrastructure.Persistence;
using RC.CA.WebUi.ActionFilters;
using Serilog;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NT.CA.WebUiMvc.Filters;
using Azure.Identity;
using RC.CA.WebMvc.HostedService;
using RC.CA.WebMvc.StartUp;
using RC.CA.Application.Startup;


// [Serilog] Setup serilog in a two-step process. First, we configure basic logging
// to be able to log errors during ASP.NET Core startup. Later, we read
// log settings from appsettings.json. Read more at
// https://github.com/serilog/serilog-aspnetcore#two-stage-initialization.
// General information about serilog can be found at
// https://serilog.net/
Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    //[Serilog] full setup take settings from application settings
    //
    builder.Host.UseSerilog((context, services, configuration) => 
                            configuration.ReadFrom.Configuration(context.Configuration)
                                         .ReadFrom.Services(services)
                                         .Enrich.FromLogContext()
                            );
    
    //[KeyVault] If production hook up key vault
    //
    if (builder.Environment.IsProduction())
        builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration.GetValue<string>("KeyVault:VaultName")), new DefaultAzureCredential());

    // Add MVC services to the container.
    //
    builder.Services.AddControllersWithViews().AddMvcOptions(options =>
    {
        //[CSP middleware] Added to support reporting of content security violations. Violations are posted with following mime type application/csp-report
        options.InputFormatters.OfType<SystemTextJsonInputFormatter>()
                               .First().SupportedMediaTypes
                               .Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/csp-report"));

        //[SeriLog] Performance logging of slow running action. And action over 4 seconds logs an entry
        options.Filters.Add(typeof(EnhancedLogging));

        //[GlobalAuthorization] Authorization is on an opt out basis. Opt out of using [AllowAnonymous] data annotation
        options.Filters.Add(new AuthorizeFilter());

        //[Authorization] Extra check on jwt token returned from api call to ensure it has not expired. Does not depend on local cookie alone.
        options.Filters.Add(typeof(JwtAuthorizationFilter));

    });

    //[DependencyInjection] Add services.  IAppContextX,IJwtUtilities is used to hydrate claims for easy access in controllers
    //
    builder.Services.AddBaseServicesUi(builder.Configuration);

    //[Automapper]
    builder.Services.AddApplicationServicesMVC(builder.Configuration); 

    //[Identity Authorization] Setup identity,
    //
    builder.Services.SetupIdentityServiceServicesMvc(builder.Configuration);

    //[CORS]
    //
    builder.Services.AddCorsPolicyMVC(builder.Configuration);

    //[HTTPClient] preferred way to use client 
    //
    builder.Services.AddHttpClient(RC.CA.SharedKernel.Constants.WebConstants.HttpFactoryName,
                                   client =>
                                   {
                                        client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiEndpoint"));
                                   });
    
    builder.Services.AddAntiforgery(options =>
    {
        options.FormFieldName = "AntiForgeryToken";
        options.HeaderName = "AntiForgeryToken";
        options.Cookie.Name = "AntiForgeryTokenCookieName";
    });

    //[Healthchecks]
    builder.Services.AddSiteHealthChecks(builder.Configuration);

    //[HostedService] Simple hosted service to delete old log files cleanup. 
    //
    builder.Services.AddHostedService<SiteUtilitiesHostedService>();

    //[Info] Middleware order execution 
    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#middleware-order
    //
    var app = builder.Build();
    
    app.Logger.LogInformation(message: $"{DateTime.Now} Adding custom middle-ware");

    //[Custom middleware] Add security headers, csp content security policy, x-frame etc...
    //
    RC.CA.WebUiMvc.Middleware.SetupMiddleware.UseSecurityHeadersMiddleWare(app);

    //[Serilog] Custom middle ware to enrich log files
    //
    app.UseLogEnrichmentMiddleware();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();
    else
    {
        //[Exception handling]
        app.UseStatusCodePagesWithReExecute("/Exceptions/Error/HttpStatusCodeErrorHandler/{0}");
        app.UseExceptionHandler("/Exceptions/Error/ErrorDetail");
    }

    //Execute order 0. Exception handler 1. HSTS 2. HttpsRedirection 3. Static files 
    //              4. Routing 5. CORS 6. Authentication 7. Authorization 8. Custom middleware
    app.UseSerilogRequestLogging(opts =>
    {
        opts.EnrichDiagnosticContext = (diagCtx, httpCtx) =>
        {
            diagCtx.Set("xMachine", Environment.MachineName);
            diagCtx.Set("xClientIP", httpCtx.Connection.RemoteIpAddress);
            diagCtx.Set("xUserAgent", httpCtx.Request.Headers["User-Agent"]);
            if (httpCtx.User.Identity?.IsAuthenticated == true)
                diagCtx.Set("UserName", httpCtx.User.Identity?.Name);
        };
    });

    app.UseHttpsRedirection(); //

    //[CORS] Order is important in pipeline.
    //
    app.UseCors(RC.CA.SharedKernel.Constants.WebConstants.CORSPolicyName);
    //app.UseCors("PublicApi");

    app.UseStaticFiles(); //

    app.UseRouting();

    app.UseAuthentication();

    //[Authentication] Hydrate app context. User claims etc...
    //                 Must execute after app.UseAuthentication() to ensure user is set.
    //
    app.UseAppContextMiddleware();

    app.UseAuthorization();

    //[Healthchecks] Configure ui
    app.ConfigureHealthChecks();
       
    //Order is important keep routes with areas above general route.
    //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/areas?view=aspnetcore-6.0
    app.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller}/{action}/{id?}");
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web site terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
