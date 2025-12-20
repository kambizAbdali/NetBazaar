using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NetBazaar.Admin.EndPoint.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IModelMetadataProvider modelMetadataProvider)
        {
            _logger = logger;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;

            _logger.LogError(ex, "Unhandled exception occurred in {ActionName}",
                context.ActionDescriptor.DisplayName);

            // هندل کردن درخواست‌های Ajax
            if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                context.Result = new JsonResult(new
                {
                    success = false,
                    error = "خطای غیرمنتظره رخ داد",
                    detailedError = ex.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            else
            {
                // بازگشت مستقیم به View بدون Redirect
                var routeData = context.RouteData;
                var controllerName = routeData.Values["controller"]?.ToString() ?? "Home";
                var actionName = routeData.Values["action"]?.ToString() ?? "Index";

                // اگر Action یک ViewResult برمی‌گرداند
                var viewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
                {
                    ["OperationError"] = $"خطا در پردازش درخواست\n{ex.Message}"
                };

                // اگر می‌خواهید مدل را هم حفظ کنید (برای POST)
                if (context.HttpContext.Items.TryGetValue("OriginalModel", out var originalModel))
                {
                    viewData.Model = originalModel;
                }

                context.Result = new ViewResult
                {
                    ViewName = actionName,
                    ViewData = viewData,
                    TempData = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>()?
                        .GetTempData(context.HttpContext)
                };

                // یا می‌توانید از این روش استفاده کنید:
                // context.Result = new ViewResult
                // {
                //     ViewName = actionName,
                //     ViewData = new ViewDataDictionary(context.Controller as ControllerBase?.ViewData)
                // };
            }

            context.ExceptionHandled = true;
            await Task.CompletedTask;
        }
    }
}