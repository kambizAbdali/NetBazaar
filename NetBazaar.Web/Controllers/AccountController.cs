using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBazaar.Domain.Entities;
using NetBazaar.Web.EndPoint.Models.ViewModels.Account;
using System.Threading.Tasks;

namespace NetBazaar.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User newUser = new User()
            {
                Name = model.Name,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return View(model);
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Login(string retutnUrl = "/")
        {
            // اصلاح نام متغیر: returnUrl
            return View(
                new LoginViewModel
                {
                    ReturnUrl = retutnUrl
                });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsPersistent, true);

            if (!result.Succeeded)
            {
                // اگر ورود موفق نبود، ممکن است ReturnUrl را به سمت Home هدایت نکند.
                // معمولاً در حالت خطا به همان View با پیام خطا برگردانده می‌شود.
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
                return View(model);
            }

            // در صورت نیاز به دو‌مرحله‌ای بودن یا سایر موارد:
            if (result.RequiresTwoFactor)
            {
                // می‌توانید به یک فریمور دو مرحله‌ای هدایت کنید
                // برای حال حاضر، به همان Home هدایت می‌کنیم
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}