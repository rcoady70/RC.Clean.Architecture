using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RC.CA.SharedKernel.Extensions;

namespace RC.CA.WebMvc.Filters.Attributes;

public class RestoreModelState : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        base.OnActionExecuting(filterContext);
        Controller controller = filterContext.Controller as Controller;
        if (controller.TempData.ContainsKey("ModelState"))
        {
            int ix = 0;
            var modelStateDictionary = (string)controller.TempData["ModelState"];
            var errors = modelStateDictionary.FromJsonExt<BaseResponseDto>();
            foreach (var error in errors.Errors)
            {
                error.Id ??=  $"AutoId_{ix++}";
                controller.ViewData.ModelState.AddModelError(error.Id, error.Detail);
            }
        }
    }
}
