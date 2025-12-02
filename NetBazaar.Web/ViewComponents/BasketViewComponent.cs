using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.Interfaces.Basket;
using NetBazaar.Web.EndPoint.Utilities;
using NetBazaar.Web.EndPoint.ViewModels;
using NetBazaar.Web.EndPoint.ViewModels.Basket;

namespace NetBazaar.Web.EndPoint.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly IBasketService _basketService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketViewComponent(IBasketService basketService, IHttpContextAccessor httpContextAccessor)
        {
            _basketService = basketService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var buyerId = BasketHelper.GetOrCreateBuyerId(
                    _httpContextAccessor.HttpContext,
                    User as System.Security.Claims.ClaimsPrincipal
                );

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
                        TotalPrice = item.TotalPrice
                    }).ToList() ?? new List<MiniBasketItemViewModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // در صورت خطا، سبد خرید خالی نمایش داده می‌شود
                return View(new MiniBasketViewModel
                {
                    ItemsCount = 0,
                    TotalPrice = 0,
                    Items = new List<MiniBasketItemViewModel>()
                });
            }
        }
    }
}