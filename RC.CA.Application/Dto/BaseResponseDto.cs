using System.Net;

namespace RC.CA.Application.Models;

/// <summary>
/// Base response for all api calls contains the error 
/// </summary>
public class BaseResponseDto
{
    public string RequestId { get; set; } = "";
    public int TotalErrors { get; set; } = 0;
    public int TotalWarnings { get; set; } = 0;
    public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    public List<ErrorModel> Warnings { get; set; } = new List<ErrorModel>();
    public HttpStatusCode RequestStatus { get; set; } = HttpStatusCode.OK;
    public enum ErrorType
    {
        Exception = 0,
        Error = 10,
        Unauthorized = 20
    }

    /// <summary>
    /// Add response error
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="Message"></param>
    public Task AddResponseError(ErrorType Type, string Message)
    {
        TotalErrors++;
        Errors.Add(new ErrorModel() { Type = Type.ToString(), Detail = Message });
        return Task.CompletedTask;
    }
    /// <summary>
    /// Add response error
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Type"></param>
    /// <param name="Message"></param>
    public Task AddResponseError(string Id, ErrorType Type, string Message)
    {
        TotalErrors++;
        Errors.Add(new ErrorModel() { Id = Id, Type = Type.ToString(), Detail = Message });
        return Task.CompletedTask;
    }


    public class ErrorModel
    {
        public string Id { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Detail { get; set; } = default!;
    }

}
