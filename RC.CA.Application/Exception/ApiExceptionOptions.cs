using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RC.CA.Application.Models;

namespace RC.CA.Infrastructure.Logging;

public class ApiExceptionOptions
{
    //Change response detail message depending on type of error. Make generic error less generic.
    public Action<HttpContext, Exception, BaseResponseDto> AddResponseDetails { get; set; }  

    //Determine log level look at excpetion content and change log level example switch to fatal error if no connection to database
    public Func<Exception, LogLevel> DetermineLogLevel { get; set; }
}

