using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBazaar.Application.DTOs.User;
using NetBazaar.Application.Interfaces.User;
using NetBazaar.Domain.Entities;
using NetBazaar.EndPoint.Controllers;

namespace NetBazaar.Web.Areas.Customers.Controllers
{
    [Area("Customers")]
    [Authorize]
    public class AddressController : BaseController
    {
        private readonly IUserAddressService _addressService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AddressController> _logger;

        public AddressController(
            IUserAddressService addressService,
            UserManager<User> userManager,
            ILogger<AddressController> logger = null)
        {
            _addressService = addressService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger?.LogWarning("User not found in Address/Index");
                    return RedirectToAction("Login", "Account");
                }

                var addresses = await _addressService.GetAddressesAsync(user.Id);
                return View(addresses);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in Address/Index");
                TempData[OperationErrorKey] = "خطا در دریافت لیست آدرس‌ها رخ داد.";
                return View(new List<UserAddressDto>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in Address/Create GET");
                TempData[OperationErrorKey] = "خطا در نمایش فرم ایجاد آدرس.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrUpdateUserAddressDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger?.LogWarning("Model validation failed in Address/Create");
                    return View(dto);
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger?.LogWarning("User not found in Address/Create POST");
                    return RedirectToAction("Login", "Account");
                }

                await _addressService.AddNewAddressAsync(user.Id, dto);
                TempData[OperationSuccessKey] = "آدرس با موفقیت ثبت شد.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in Address/Create POST for user");
                TempData[OperationErrorKey] = $"ثبت آدرس با خطا مواجه شد: {GetErrorMessage(ex)}";
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger?.LogWarning("User not found in Address/Edit GET");
                    return RedirectToAction("Login", "Account");
                }

                var address = await _addressService.GetAddressByIdAsync(id, user.Id);
                if (address == null)
                {
                    _logger?.LogWarning("Address not found. ID: {AddressId}, User: {UserId}", id, user.Id);
                    TempData[OperationErrorKey] = "آدرس مورد نظر یافت نشد.";
                    return RedirectToAction("Index");
                }

                var editDto = new CreateOrUpdateUserAddressDto
                {
                    ReceiverName = address.ReceiverName,
                    PhoneNumber = address.PhoneNumber,
                    PostalCode = address.PostalCode,
                    AddressText = address.AddressText,
                    IsDefault = address.IsDefault
                };

                ViewBag.AddressId = id;
                return View(editDto);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in Address/Edit GET for address ID: {AddressId}", id);
                TempData[OperationErrorKey] = "خطا در نمایش فرم ویرایش آدرس.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateOrUpdateUserAddressDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger?.LogWarning("Model validation failed in Address/Edit for address ID: {AddressId}", id);
                    ViewBag.AddressId = id;
                    return View(dto);
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger?.LogWarning("User not found in Address/Edit POST");
                    return RedirectToAction("Login", "Account");
                }

                await _addressService.UpdateAddressAsync(id, user.Id, dto);
                TempData[OperationSuccessKey] = "آدرس با موفقیت ویرایش شد.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in Address/Edit POST for address ID: {AddressId}", id);
                TempData[OperationErrorKey] = $"ویرایش آدرس با خطا مواجه شد: {GetErrorMessage(ex)}";
                ViewBag.AddressId = id;
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger?.LogWarning("User not found in Address/Delete");
                    return RedirectToAction("Login", "Account");
                }

                await _addressService.DeleteAddressAsync(id, user.Id);
                TempData[OperationSuccessKey] = "آدرس با موفقیت حذف شد.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in Address/Delete for address ID: {AddressId}", id);
                TempData[OperationErrorKey] = $"حذف آدرس با خطا مواجه شد: {GetErrorMessage(ex)}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetDefaultAddressAsync(int addressId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger?.LogWarning("User not found in SetDefaultAddressAsync");
                    return RedirectToAction("Login", "Account");
                }

                if (addressId <= 0)
                {
                    _logger?.LogWarning("Invalid address ID in SetDefaultAddressAsync: {AddressId}", addressId);
                    TempData[OperationErrorKey] = "شناسه آدرس نامعتبر است.";
                    return RedirectToAction("Index");
                }

                await _addressService.SetDefaultAddressAsync(addressId, user.Id);
                TempData[OperationSuccessKey] = "آدرس پیش‌فرض با موفقیت تغییر یافت.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in SetDefaultAddressAsync for address ID: {AddressId}", addressId);
                TempData[OperationErrorKey] = $"تغییر آدرس پیش‌فرض با خطا مواجه شد: {GetErrorMessage(ex)}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// تبدیل خطاهای سیستمی به پیام‌های کاربرپسند
        /// </summary>

    }
}