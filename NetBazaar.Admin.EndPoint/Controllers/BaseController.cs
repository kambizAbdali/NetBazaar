using Microsoft.AspNetCore.Mvc;

namespace NetBazaar.Admin.EndPoint.Controllers
{
    public class BaseController : Controller
    {
        protected const string OperationErrorKey = "OperationError";
        protected const string OperationSuccessKey = "SuccessMessage";
        protected const int DefaultPageSize = 10;
    }
}
