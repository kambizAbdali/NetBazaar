using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Events;
using NetBazaar.Application.DTOs.Basket;
using NetBazaar.Application.Interfaces.Basket;
using NetBazaar.Domain.Entities.Basket;
using NetBazaar.EndPoint.Controllers;
using NetBazaar.Web.EndPoint.Utilities;
using NetBazaar.Web.EndPoint.ViewModels.Basket;
using NuGet.ContentModel;
using System.Security.Claims;

namespace NetBazaar.Web.EndPoint.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketController(IBasketService basketService, IHttpContextAccessor httpContextAccessor)
        {
            _basketService = basketService;
            _httpContextAccessor = httpContextAccessor;
        }

        private static BasketDto basket;
        private static string buyerId;
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                buyerId = await GetOrCreateBuyerIdAsync();
                basket = await _basketService.GetBasketForUserAsync(buyerId);

                if (basket == null || !basket.Items.Any())
                {
                    return View(new BasketViewModel
                    {
                        Items = new List<BasketItemViewModel>(),
                        IsEmpty = true
                    });
                }

                var viewModel = MapToViewModel(basket);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در بارگذاری سبد خرید";
                return View(new BasketViewModel { IsEmpty = true });
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> AddItem(int catalogItemId, int quantity = 1)
        //{
        //    try
        //    {
        //        if (quantity <= 0)
        //        {
        //            TempData["ErrorMessage"] = "تعداد باید بیشتر از صفر باشد";
        //            return RedirectToAction("ProductDetail", "Product", new { id = catalogItemId });
        //        }

        //        var buyerId = await GetOrCreateBuyerIdAsync();
        //        var basket = await _basketService.GetOrCreateBasketForUserAsync(buyerId);

        //        await _basketService.AddItemToBasketAsync(basket.Id, catalogItemId, quantity);

        //        TempData["SuccessMessage"] = "محصول به سبد خرید اضافه شد";
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "خطا در افزودن محصول به سبد خرید";
        //        return RedirectToAction("ProductDetail", "Product", new { id = catalogItemId });
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int basketItemId, int quantity)
        {
            try
            {
                if (quantity <= 0)
                {
                    return Json(new { success = false, message = "تعداد باید بیشتر از صفر باشد" });
                }

                await _basketService.SetItemQuantityAsync(basketItemId, quantity);

                // دریافت اطلاعات به‌روز شده سبد خرید
                var buyerId = await GetOrCreateBuyerIdAsync();
                var basket = await _basketService.GetBasketForUserAsync(buyerId);

                var item = basket?.Items.FirstOrDefault(i => i.Id == basketItemId);
                if (item == null)
                {
                    return Json(new { success = false, message = "آیتم یافت نشد" });
                }

                return Json(new
                {
                    success = true,
                    itemTotalPrice = item.TotalPrice.ToString("N0"),
                    basketTotalPrice = basket.TotalPrice.ToString("N0"),
                    totalItems = basket.TotalItems
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "خطا در به‌روزرسانی تعداد" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int basketItemId)
        {
            try
            {
                 await _basketService.RemoveItemFromBasketAsync(basketItemId);
                 basket = await _basketService.GetBasketForUserAsync(buyerId);
                var items = basket.Items.ToList();
                TempData["SuccessMessage"] = "محصول از سبد خرید حذف شد";
                return Json(new
                {
                    success = true,
                    basketTotalPrice = basket.TotalPrice.ToString("N0"),
                    totalItems = basket.TotalItems
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در حذف محصول از سبد خرید";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            try
            {
                var buyerId = await GetOrCreateBuyerIdAsync();
                var basket = await _basketService.GetBasketForUserAsync(buyerId);

                if (basket != null)
                {
                    await _basketService.ClearBasketAsync(basket.Id);
                    TempData["SuccessMessage"] = "سبد خرید خالی شد";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در خالی کردن سبد خرید";
            }

            return RedirectToAction("Index");
        }

        // متدهای کمکی خصوصی
        private async Task<string> GetOrCreateBuyerIdAsync()
        {
            // اگر کاربر لاگین کرده باشد، از UserId استفاده می‌کنیم
            if (User.Identity.IsAuthenticated)
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            // اگر کاربر لاگین نکرده باشد، از کوکی استفاده می‌کنیم
            var buyerId = _httpContextAccessor.HttpContext.Request.Cookies["BasketBuyerId"];

            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    IsEssential = true
                };
                _httpContextAccessor.HttpContext.Response.Cookies.Append("BasketBuyerId", buyerId, cookieOptions);
            }

            return buyerId;
        }

        private BasketViewModel MapToViewModel(BasketDto basket)
        {
            return new BasketViewModel
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemViewModel
                {
                    Id = item.Id,
                    CatalogItemId = item.CatalogItemId,
                    Name = item.CatalogItemName,
                    ImageUrl = item.CatalogItemImageUrl,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice,
                    BrandName = item.BrandName,
                    CategoryName = item.CategoryName,
                    AvailableStock = item.AvailableStock
                }).ToList(),
                TotalPrice = basket.TotalPrice,
                TotalItems = basket.TotalItems,
                IsEmpty = !basket.Items.Any()
            };
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int catalogItemId, int quantity = 1, string returnUrl = null)
        {
            try
            {
                if (quantity <= 0)
                {
                    return Json(new { success = false, message = "تعداد باید بیشتر از صفر باشد" });
                }

                var buyerId = await GetOrCreateBuyerIdAsync();
                var basket = await _basketService.GetOrCreateBasketForUserAsync(buyerId);

                // Check stock availability
                var stockValidationResult = await ValidateStockAvailability(basket, catalogItemId, quantity);
                if (!stockValidationResult.IsValid)
                {
                    TempData["Error"] = stockValidationResult.ErrorMessage;
                    return HandleResponse(returnUrl, false);
                }
                try
                {
// Add item to basket
                await _basketService.AddItemToBasketAsync(basket.Id, catalogItemId, quantity);
                }
                catch (Exception ex)
                {

                    throw;
                }
                

                TempData["Success"] = "محصول با موفقیت به سبد خرید افزوده شد";
                return HandleResponse(returnUrl, true, basket);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "خطا در افزودن محصول به سبد خرید";
                return HandleResponse(returnUrl, false);
            }
        }

        private async Task<StockValidationResult> ValidateStockAvailability(BasketDto basket, int catalogItemId, int quantity)
        {
            var existingItem = basket.Items.FirstOrDefault(item => item.CatalogItemId == catalogItemId);
            var currentQuantityInCart = existingItem?.Quantity ?? 0;
            var requestedTotalQuantity = currentQuantityInCart + quantity;

            if (existingItem == null || requestedTotalQuantity <= existingItem.AvailableStock)
            {
                return new StockValidationResult { IsValid = true };
            }

            var availableQuantity = existingItem.AvailableStock - currentQuantityInCart;

            if (availableQuantity <= 0)
            {
                return new StockValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "شما حداکثر تعداد قابل خرید این محصول را درحال حاضر در سبد خرید دارید"
                };
            }

            return new StockValidationResult
            {
                IsValid = false,
                ErrorMessage = $"فقط {availableQuantity} عدد از این محصول در انبار موجود است"
            };
        }

        private IActionResult HandleResponse(string returnUrl, bool success, BasketDto basket = null)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            var message = success ? TempData["Success"]?.ToString() : TempData["Error"]?.ToString();

            return Json(new
            {
                success,
                message,
                basketItemsCount = basket?.TotalItems ?? 0,
                totalPrice = basket?.TotalPrice ?? 0
            });
        }

        private class StockValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
        }

        private IActionResult HandleSuccess(string message, int basketItemsCount, string returnUrl)
        {
            TempData["Success"] = message;

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Json(new
            {
                success = true,
                message = message,
                basketItemsCount = basketItemsCount
            });
        }

        private IActionResult HandleError(string message, string returnUrl)
        {
            TempData["Error"] = message;

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Json(new
            {
                success = false,
                message = message
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetMiniBasket()
        {
            try
            {
                var buyerId = await GetOrCreateBuyerIdAsync();
                var basket = await _basketService.GetBasketForUserAsync(buyerId);

                var viewModel = new MiniBasketViewModel
                {
                    ItemsCount = basket?.TotalItems ?? 0,
                    TotalPrice = basket?.TotalPrice ?? 0,
                    Items = basket?.Items.Take(3).Select(item => new MiniBasketItemViewModel
                    {
                        Id = item.Id,
                        CatalogItemId = item.CatalogItemId,
                        Name = item.CatalogItemName,
                        ImageUrl = item.CatalogItemImageUrl,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice,
                    }).ToList() ?? new List<MiniBasketItemViewModel>()
                };

                return PartialView("Components/Basket/Default", viewModel);
            }
            catch (Exception ex)
            {
                // در صورت خطا، سبد خرید خالی نمایش داده می‌شود
                return PartialView("Components/Basket/Default", new MiniBasketViewModel
                {
                    ItemsCount = 0,
                    TotalPrice = 0,
                    Items = new List<MiniBasketItemViewModel>()
                });
            }
        }
    }
}