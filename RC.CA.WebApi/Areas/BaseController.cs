using Microsoft.AspNetCore.Mvc;

namespace RC.CA.WebApi.Areas;

public abstract class BaseController : ControllerBase
{

    /// <summary>
    /// Handle action result return correct action result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    protected ActionResult<T> HandleResult<T>(T result) where T : ICAResult
    {
        switch (result.Status)
        {
            case ResultStatus.Ok: return Ok(result);
            case ResultStatus.NotFound: return NotFound(result);
            case ResultStatus.Unauthorized: return Unauthorized(result);
            case ResultStatus.Forbidden: return Forbid();
            case ResultStatus.BadRequest: return BadRequest(result);
            case ResultStatus.ServerError: return UnprocessableEntity(result);
            default: throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
        }
    }
}
