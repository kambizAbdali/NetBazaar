using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace NetBazaar.Web.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }        
    }
}