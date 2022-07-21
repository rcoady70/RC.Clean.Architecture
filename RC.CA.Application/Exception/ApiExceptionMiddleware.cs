using System.Net;
using System.Security;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RC.CA.Infrastructure.Logging.Middleware.Exceptions;

/// Exception middleware. Note use action injection for DI components. 
///                       Middleware is invoked as singleton so cannot use constructor injection on scoped or transient scoped objects.
public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;
    private readonly ApiExceptionOptions _options;


    public ApiExceptionMiddleware(ApiExceptionOptions options, RequestDelegate next,
                                   ILogger<ApiExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _options = options;
    }

    public async Task Invoke(HttpContext context)
    {
        //Catch any exceptions and handle them
        //
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    /// <summary>
    /// Handle the exception
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        //Try to get requestId from header. Used to tie error messages between mvc site and api site
        if (!context.Request.Headers.TryGetValue(WebConstants.CorrelationId, out var correlationId))
            correlationId = "Not-Found";


        List<ValidationError> validationErrors = new List<ValidationError>();

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        //If known error give more information
        switch (exception)
        {
            case ArgumentException:
                validationErrors.Add(new ValidationError() { ErrorCode = correlationId, ErrorMessage = $"Invalid argument. {exception.Message.EscapeHtmlExt()} Error id {correlationId}.", Severity = ValidationSeverity.Exception });
                break;
            case SecurityException:
                validationErrors.Add(new ValidationError() { ErrorCode = correlationId, ErrorMessage = $"Security exception. {exception.Message.EscapeHtmlExt()} Error id {correlationId}.", Severity = ValidationSeverity.Exception });
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            default:
                validationErrors.Add(new ValidationError() { ErrorCode = correlationId, ErrorMessage = $"Unhanded exception occurred. Please contact support if the problem persists and include the error id {correlationId}.", Severity = ValidationSeverity.Exception });
                break;
        }

        //Internal log. Set level depending on error. For example database full, network error switch error to critical
        //
        var level = _options.DetermineLogLevel?.Invoke(exception) ?? LogLevel.Error;
        var innerExMessage = GetInnermostExceptionMessage(exception);
        _logger.Log(level, exception,
                    $"API exception" + innerExMessage.Replace("{", @"[").Replace("}", @"]") + " error id {correlationId}.",
                    correlationId);

        // Public error message returned in api response
        //
        var result = JsonSerializer.Serialize(CAResultEmpty.Invalid(validationErrors));
        await context.Response.WriteAsync(result);
    }
    /// <summary>
    /// Get inner exception
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    private string GetInnermostExceptionMessage(Exception exception)
    {
        if (exception.InnerException != null)
            return GetInnermostExceptionMessage(exception.InnerException);

        return exception.Message;
    }
}

