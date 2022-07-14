namespace RC.CA.Application.Models;

/// <summary>
/// Re-factored using CAResult. Base response for all api calls contains the error 
/// </summary>
public class BaseResponseCAResult
{
    public string RequestId { get; set; } = "";

}
