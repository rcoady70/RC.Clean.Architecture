using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace RC.CA.WebApi.Areas;

public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Return invalid result.Sets status code to 400 bad request
    /// </summary>
    /// <typeparam name="T">Result object</typeparam>
    /// <returns>Result object</returns>
    internal T InvalidRequest<T>(T result)
    {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return result;
    }
    
}
