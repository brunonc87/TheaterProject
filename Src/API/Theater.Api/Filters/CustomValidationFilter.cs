using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Theater.Api.Filters
{
    public class CustomValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                       .SelectMany(ms => ms.Value.Errors)
                       .Select(e => e.ErrorMessage)
                       .ToList();

                string errorMessage = string.Empty;

                foreach (var error in errors)
                {
                    errorMessage += error + ".\r\n";
                }

                context.Result = new BadRequestObjectResult(errorMessage);
            }
        }
    }
}
