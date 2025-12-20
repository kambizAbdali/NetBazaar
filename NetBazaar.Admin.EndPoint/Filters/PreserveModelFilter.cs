using Microsoft.AspNetCore.Mvc.Filters;

namespace NetBazaar.Admin.EndPoint.Filters
{
    public class PreserveModelFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // ذخیره مدل اصلی در HttpContext.Items
            if (context.ActionArguments.Count > 0)
            {
                context.HttpContext.Items["OriginalModel"] = context.ActionArguments.Values.FirstOrDefault();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // اجرای پس از اکشن
        }
    }
}