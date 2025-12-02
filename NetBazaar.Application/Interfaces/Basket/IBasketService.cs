using NetBazaar.Application.DTOs.Basket;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Basket
{
    public interface IBasketService
    {
        // دریافت یا ایجاد سبد خرید برای کاربر
        Task<BasketDto> GetOrCreateBasketForUserAsync(string buyerId);

        // فقط دریافت سبد خرید (اگر وجود نداشت null برمی‌گرداند)
        Task<BasketDto> GetBasketForUserAsync(string buyerId);

        // افزودن آیتم به سبد خرید
        Task AddItemToBasketAsync(int basketId, int catalogItemId, int quantity);

        // تغییر تعداد آیتم در سبد خرید
        Task SetItemQuantityAsync(int basketItemId, int quantity);

        // حذف آیتم از سبد خرید
        Task RemoveItemFromBasketAsync(int basketItemId);
        // انتقال سبد خرید از کاربر ناشناس به کاربر لاگین‌شده
        Task TransferBasketAsync(string anonymousBuyerId, string userBuyerId);

        // خالی کردن سبد خرید
        Task ClearBasketAsync(int basketId);

        // دریافت تعداد آیتم‌های سبد خرید
        Task<int> GetBasketItemsCountAsync(string buyerId);
        Task<MiniBasketDto> GetMiniBasketAsync(string buyerId);
        Task<BasketDto> GetBasketByIdAsync(int basketId);
        Task RemoveItemsFromBasketAsync(List<int> basketItemIds);

    }
}
