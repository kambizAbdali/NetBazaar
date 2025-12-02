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
    }
}
