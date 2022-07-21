using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RC.CA.WebMvc.Filters.Attributes;
/// <summary>
/// Save model state to temp data for one response
/// </summary>
public class SaveModelState : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        base.OnActionExecuted(filterContext);
        Controller controller = filterContext.Controller as Controller;
        if (!controller.ViewData.ModelState.IsValid)
        {
            int ix = 0;
            List<ValidationError> validationErrors = new List<ValidationError>();
            foreach (var modelError in controller.ViewData.ModelState)
            {
                foreach (var error in modelError.Value.Errors)
                    validationErrors.Add(new ValidationError { ErrorCode = $"AutoId_{ix++}", ErrorMessage = error.ErrorMessage, Severity = ValidationSeverity.Error });
            }
            controller.TempData["ModelState"] = CAResultEmpty.Invalid(validationErrors).ToJsonExt();
        }
    }
}
