using System.Net;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Dto;

namespace RC.CA.WebApi.Areas;

public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Alternative approach generics over inheritance give BaseResponseDto. Drop result into generic response.
    /// Return invalid result.Sets status code to 400 bad request
    /// </summary>
    /// <typeparam name="T">Result object</typeparam>
    /// <returns>Result object</returns>
    internal T InvalidRequest<T>(T result)
    {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return result;
    }
    /// <summary>
    /// Handle result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    protected ActionResult HandleResult<T>(ApiResultDto<T> result)
    {
        if (result == null)
            return NotFound(result);
        if (result.IsSuccess)
            return Ok(result);
        if (result.IsSuccess && result.Result == null)
            return NotFound(result);
        return BadRequest(result);
    }
}
