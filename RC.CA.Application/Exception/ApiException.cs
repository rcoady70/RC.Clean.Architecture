using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Application.Exceptions;
/// <summary>
/// Api exception
/// </summary>
public partial class ApiException : System.Exception
{
    public int StatusCode { get; private set; }
    public string Response { get; private set; }
    public string Request { get; private set; }
    public string RequestId { get; private set; }
    public string Hostname { get; private set; }

    public ApiException(string requestId, string endpoint,string message, int statusCode,string request, string response,System.Exception? innerException)
                        : base($"Endpoint:\n{endpoint}\nCorrelationID:\n{requestId}\nMessage:\n{message}\nStatus: {statusCode}\nRequest:\n{request}\nResponse:\n{response}",
                        innerException)
    {
        StatusCode = statusCode;
        Response = response;
        Request = request;
        RequestId = requestId;
        Hostname = Environment.MachineName;
    }

    public override string ToString()
    {
        return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
    }
}
