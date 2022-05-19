using System.Text.Json;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using RC.CA.SharedKernel.Constants;
using RC.CA.Application.Models;
using RC.CA.SharedKernel.Extensions;
using RC.CA.Application.Exceptions;
using System.Security;

namespace RC.CA.Infrastructure.Logging.Middleware.Exceptions;

/// Exception middleware. Note use action injection for DI components. 
///                       Middleware is invoked as singleton so cannot use constructor injection on scoped or transient scoped objects.
public class ApiExceptionMiddleware
{
private readonly RequestDelegate _next;
private readonly ILogger<ApiExceptionMiddleware> _logger;
private readonly ApiExceptionOptions _options;
    

public ApiExceptionMiddleware( ApiExceptionOptions options, RequestDelegate next,
	                           ILogger<ApiExceptionMiddleware> logger )
{
	_next = next;
	_logger = logger;
	_options = options;
}

public async Task Invoke( HttpContext context )
{
    //Catch any exceptions and handle them
    //
	try
	{
		await _next( context );
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
private async Task HandleExceptionAsync( HttpContext context, Exception exception)
{
        //Try to get requestId from header. Used to tie error messages between mvc site and api site
        if (!context.Request.Headers.TryGetValue(WebConstants.CorrelationId, out var correlationId))
            correlationId = "Not-Found";

        var error = new BaseResponseDto
        {
            RequestStatus = HttpStatusCode.InternalServerError,
            RequestId = correlationId
        };
       
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        //If known error give more information
        if (exception is ArgumentException)
            error.AddResponseError(correlationId, BaseResponseDto.ErrorType.Exception, $"Invalid argument. {exception.Message.EscapeHtmlExt()} Error id {correlationId}.");
        else if (exception is SecurityException)
        {
            error.AddResponseError(correlationId, BaseResponseDto.ErrorType.Exception, $"Security exception. {exception.Message.EscapeHtmlExt()} Error id {correlationId}.");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            error.RequestStatus = HttpStatusCode.Unauthorized;
        }
        else
            error.AddResponseError(correlationId, BaseResponseDto.ErrorType.Exception, $"Unhanded exception occurred. Please contact support if the problem persists and include the error id {correlationId}.");

        //Change log level depending on error. For example database full, network error switch error to critical
        var level = _options.DetermineLogLevel?.Invoke(exception) ?? LogLevel.Error;
        //Supplement response details public error message
        _options.AddResponseDetails?.Invoke(context, exception, error);

        var innerExMessage = GetInnermostExceptionMessage(exception);
        _logger.Log(level, exception,
                    $"API exception" + innerExMessage.Replace("{", @"[").Replace("}", @"]") + " error id {correlationId}.",
                    correlationId);

        //Public error message returned in api response
        //
        var result = JsonSerializer.Serialize(error);
        await context.Response.WriteAsync(result);
    }
    /// <summary>
    /// Get inner exception
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    private string GetInnermostExceptionMessage( Exception exception )
    {
	    if ( exception.InnerException != null )
		    return GetInnermostExceptionMessage( exception.InnerException );

	    return exception.Message;
    }
}

