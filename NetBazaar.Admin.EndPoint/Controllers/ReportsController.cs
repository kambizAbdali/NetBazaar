using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace NetBazaar.Web.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }        
    }
}