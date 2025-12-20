using Microsoft.AspNetCore.Mvc;

namespace NetBazaar.Admin.EndPoint.Controllers
{
    public class BaseController : Controller
    {
        public const string OperationErrorKey = "OperationError";
        public const string OperationSuccessKey = "SuccessMessage";
        public const int DefaultPageSize = 10;
    }
}
