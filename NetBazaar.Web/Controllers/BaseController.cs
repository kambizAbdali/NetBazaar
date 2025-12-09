using Microsoft.AspNetCore.Mvc;

namespace NetBazaar.EndPoint.Controllers
{
    public class BaseController : Controller
    {
        protected const string OperationErrorKey = "OperationError";
        protected const string OperationSuccessKey = "SuccessMessage";
        protected const int DefaultPageSize = 10;

        protected void HideMenuCategories()
        {
            TempData["HideMenuCategories"] = true;
        }

        protected void ShowMenuCategories()
        {
            TempData["HideMenuCategories"] = false;
        }

        protected string GetErrorMessage(Exception ex)
        {
            // لاگ کردن خطای کامل برای توسعه‌دهندگان
            // _logger?.LogError(ex, "System error occurred");

            // بررسی نوع خطا و بازگرداندن پیام مناسب
            return ex switch
            {
                UnauthorizedAccessException => "دسترسی غیرمجاز.",
                ArgumentNullException => "اطلاعات ارسالی ناقص است.",
                InvalidOperationException => "عملیات درخواستی مجاز نمی‌باشد.",
                KeyNotFoundException => "مورد درخواستی یافت نشد.",
                TimeoutException => "زمان عملیات به پایان رسید. لطفاً مجدداً تلاش کنید.",
                _ => $"عملیات با خطا مواجه شد. لطفاً مجدداً تلاش کنید:\n{ex.Message}."
            };
        }
    }
}
