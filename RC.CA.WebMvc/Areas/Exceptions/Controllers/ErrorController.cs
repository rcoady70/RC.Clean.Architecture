using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Infrastructure.Logging.Constants;
using RC.CA.WebUiMvc.Models;
using RC.CA.SharedKernel.Extensions;
using Serilog.Context;

namespace RC.CA.WebUiMvc.Areas.Exceptions.Controllers;
[AllowAnonymous]
[Area("Exceptions")]
public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;
    private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

    public ErrorController(ILogger<ErrorController> logger,
                           Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
    {
        _logger = logger;
        _hostingEnvironment = hostingEnvironment;
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult HttpStatusCodeErrorHandler(int statusCode)
    {
        var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
        switch (HttpContext.Response.StatusCode)
        {
            case 404:
                _logger.LogWarning(LoggerEvents.Warning404Evt, @"404: Path: {Path}  Querystring: {Querystring} ", statusCodeResult.OriginalPath, statusCodeResult.OriginalQueryString);
                break;
        }
        ViewBag.Status = HttpContext.Response.StatusCode;
        return View(nameof(HttpStatusCodeErrorHandler));
    }
    /// <summary>
    /// Error controller
    /// </summary>
    /// <returns></returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ErrorDetail()
    {
        var errorModel = new ErrorViewModel();
        var requestId = Activity.Current?.Id;
        if (!string.IsNullOrEmpty(requestId))
            requestId = HttpContext.TraceIdentifier;
        //
        var userName = "";
        if (User?.Identity?.Name != null)
            userName = User.Identity.Name;
        //Get exception
        //
        var exceptionPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var ex = exceptionPathFeature?.Error;
        if(ex.Data.Contains("ApiEndpoint"))
            errorModel.ApiRoute = ex.Data["ApiEndpoint"]?.ToString();

        errorModel.RequestId = requestId;
        errorModel.UserName = userName;

        if (_hostingEnvironment.IsDevelopment())
        {
            errorModel.DelevoperMessage = ex.Message;
            errorModel.DelevoperStackTrace = ex.StackTrace;
        }
        return View(errorModel);
    }
}

