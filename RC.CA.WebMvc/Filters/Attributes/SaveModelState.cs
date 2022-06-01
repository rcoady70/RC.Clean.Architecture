using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RC.CA.WebMvc.Filters.Attributes;

public class SaveModelState : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        base.OnActionExecuted(filterContext);
        Controller controller = filterContext.Controller as Controller;
        if (!controller.ViewData.ModelState.IsValid)
        {
            int ix = 0;
            BaseResponseDto responseDto = new BaseResponseDto();
            foreach (var modelError in controller.ViewData.ModelState)
            {
                foreach(var error in modelError.Value.Errors)
                    responseDto.AddResponseError($"AutoId_{ix++}",BaseResponseDto.ErrorType.Error, error.ErrorMessage);
            }
            controller.TempData["ModelState"] = responseDto.ToJsonExt();
        }
    }
}
