using Microsoft.AspNetCore.Mvc;

namespace RC.CA.WebApi.Areas;

public abstract class BaseController : ControllerBase
{

    /// <summary>
    /// Handel request wrap as Iaction result
    /// </summary>
    /// <typeparam name="T">CAResult</typeparam>
    /// <param name="result">CAResult returned by mediatr</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    protected ActionResult HandleResult<T>(CAResult<T> result)
    {
        switch (result.Status)
        {
            case ResultStatus.Ok: return typeof(T).IsInstanceOfType(result) ? (ActionResult)Ok() : Ok(result);
            case ResultStatus.NotFound: return NotFound();
            case ResultStatus.Unauthorized: return Unauthorized();
            case ResultStatus.Forbidden: return Forbid();
            case ResultStatus.Invalid: return BadRequest(result);
            case ResultStatus.Error: return UnprocessableEntity(result);
            default: throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
        }
    }
}
