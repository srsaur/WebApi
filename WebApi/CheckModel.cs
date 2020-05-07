using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApi
{
    public class CheckModel : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null && context.HttpContext.Response.StatusCode == 200)
            {
                context.Result = new ObjectResult(new { Success = true, result = context.Result is EmptyResult ? null : context.Result.GetType().GetProperty("Value").GetValue(context.Result) });
            }
        }
    }
}
