using Microsoft.Extensions.Logging;

namespace RC.CA.Infrastructure.Logging;

public class ApiExceptionOptions
{
    //Determine log level look at exception content and change log level example switch to fatal error if no connection to database
    public Func<Exception, LogLevel> DetermineLogLevel { get; set; }
}

