using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RC.CA.WebMvc.Filters.Attributes;

public class RestoreModelState : ActionFilterAttribute
{
    /// <summary>
    /// Restore model state from temp data uses [RestoreModelState] data annotation
    /// </summary>
    /// <param name="filterContext"></param>
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        base.OnActionExecuting(filterContext);
        Controller controller = filterContext.Controller as Controller;
        if (controller.TempData.ContainsKey("ModelState"))
        {
            int ix = 0;
            var modelStateDictionary = (string)controller.TempData["ModelState"];
            CAResultEmpty errors = modelStateDictionary.FromJsonExt<CAResultEmpty>();
            foreach (var error in errors.ValidationErrors)
            {
                error.ErrorCode ??= $"AutoId_{ix++}";
                controller.ViewData.ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
            }
        }
    }
}
