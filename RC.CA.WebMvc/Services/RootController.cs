using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Contracts.Identity;

namespace RC.CA.WebUiMvc.Services;
/// <summary>
/// Root controller contains mvc controller helper functions
/// </summary>
public abstract class RootController : Controller
{
    internal readonly IAppContextX _appContextX;

    /// <summary>
    /// Application context, contains authorization info user etc...
    /// </summary>
    /// <param name="appContextX"></param>
    public RootController(IAppContextX appContextX)
    {
        _appContextX = appContextX;
    }

    /// <summary>
    /// Helper function to apply errors returned from api (CAResult) to model state 
    /// </summary>
    /// <param name="response">Base response from api call</param>
    /// <returns></returns>
    public async Task AppendErrorsToModelStateAsyncCAResult(List<ValidationError>? errors)
    {

        if (errors != null)
        {
            foreach (var error in errors)
                ModelState.AddModelError("", error.ErrorMessage);
        }
    }
}
