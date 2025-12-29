using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Core.Misc;
using NetBazaar.Application.DTOs.Basket;
using NetBazaar.Application.Interfaces;
using NetBazaar.Application.Interfaces.Basket;
using NetBazaar.Domain.Entities.Basket;
using NetBazaar.Infrastructure.Services.Catalog;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NetBazaar.Infrastructure.Services.Basket
{
    public class BasketService : IBasketService
    {
        private readonly INetBazaarDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageUrlService _imageUrlService;
        private readonly IDiscountService _discountService; // New code

        public BasketService(INetBazaarDbContext context, IMapper mapper, IImageUrlService imageUrlService, IDiscountService discountService)
        {
            _context = context;
            _mapper = mapper;
            _imageUrlService = imageUrlService;
            _discountService = discountService;
        }

        public async Task<BasketDto> GetOrCreateBasketForUserAsync(string buyerId)
        {
            var basket = await GetBasketByBuyerIdAsync(buyerId);

            if (basket == null)
            {
                basket = await CreateBasketForUserAsync(buyerId);
            }

            return await MapBasketToDtoAsync(basket);
        }

        public async Task<BasketDto> GetBasketForUserAsync(string buyerId)
        {
            var basket = await GetBasketByBuyerIdAsync(buyerId);
            return basket != null ? await MapBasketToDtoAsync(basket) : null;
        }

        public async Task<BasketDto> GetBasketByIdAsync(int basketId)
        {
            var basketQuery = await _context.Baskets
                .Where(b => b.Id == basketId)
                .Select(b => new BasketDto
                {
                    Id = b.Id,
                    BuyerId = b.BuyerId,
                    Items = b.Items
                        .Where(item => !item.IsRemoved)
                        .Select(item => new BasketItemDto
                        {
                            Id = item.Id,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            CatalogItemId = item.CatalogItemId,
                            CatalogItemName = item.CatalogItem.Name,
                            BrandName = item.CatalogItem.CatalogBrand.Brand,
                            CategoryName = item.CatalogItem.CatalogType.Type,
                            CatalogItemImageUrl = item.CatalogItem.Images
                                .OrderBy(img => img.SortOrder)
                                .Select(img => img.Src)
                                .FirstOrDefault()
                        }).ToList()
                })
                .FirstOrDefaultAsync();


            // نرمال‌سازی تصاویر برای هر آیتم کاتالوگ
            foreach (var item in basketQuery.Items)
                if (item.CatalogItemImageUrl.IsNullOrEmpty())
                    item.CatalogItemImageUrl = _imageUrlService.Normalize(item?.CatalogItemImageUrl, ImageType.Product);

            return basketQuery;
        }


        public async Task AddItemToBasketAsync(int basketId, int catalogItemId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            var basket = await _context.Baskets
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == basketId);

            if (basket == null)
                throw new ArgumentException($"Basket with id {basketId} not found", nameof(basketId));

            // دریافت قیمت محصول از دیتابیس
            var catalogItem = await _context.Catalogs
                .FirstOrDefaultAsync(ci => ci.Id == catalogItemId);

            if (catalogItem == null)
                throw new ArgumentException($"Catalog item with id {catalogItemId} not found", nameof(catalogItemId));

            var unitPrice = (decimal)catalogItem.Price;


            var basketItem = await _context.BasketItems
      .AsNoTracking()
      .FirstOrDefaultAsync(o => o.BasketId == basketId && o.CatalogItemId == catalogItemId);



            // افزودن آیتم به سبد خرید
            basket.AddItem(catalogItemId, quantity, unitPrice);

            await _context.SaveChangesAsync();
        }

        public async Task SetItemQuantityAsync(int basketItemId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            var basketItem = await _context.BasketItems
                .FirstOrDefaultAsync(bi => bi.Id == basketItemId);

            if (basketItem == null)
                throw new ArgumentException($"Basket item with id {basketItemId} not found", nameof(basketItemId));

            basketItem.SetQuantity(quantity);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromBasketAsync(int basketItemId)
        {
            // حذف مستقیم در دیتابیس بدون لود کردن entity
            var rowsAffected = await _context.BasketItems
                .Where(bi => bi.Id == basketItemId)
                .ExecuteDeleteAsync();

            if (rowsAffected == 0)
                throw new ArgumentException($"آیتم سبد خرید با شناسه {basketItemId} یافت نشد", nameof(basketItemId));
        }
        public async Task RemoveItemsFromBasketAsync(List<int> basketItemIds)
        {
            if (basketItemIds == null || !basketItemIds.Any())
                throw new ArgumentException("لیست شناسه‌های آیتم‌های سبد خرید نمی‌تواند خالی باشد");

            // حذف مستقیم در دیتابیس بدون لود کردن entity
            var rowsAffected = await _context.BasketItems
                .Where(bi => basketItemIds.Contains(bi.Id))
                .ExecuteDeleteAsync();

            if (rowsAffected == 0)
                throw new ArgumentException("هیچ آیتم سبد خرید با شناسه‌های ارائه شده یافت نشد");
        }
        public async Task TransferBasketAsync(string anonymousBuyerId, string userBuyerId)
        {
            var anonymousBasket = await GetBasketByBuyerIdAsync(anonymousBuyerId);

            if (anonymousBasket == null || !anonymousBasket.Items.Any())
                return; // هیچ سبد خریدی برای انتقال وجود ندارد

            var userBasket = await GetBasketByBuyerIdAsync(userBuyerId);

            if (userBasket == null)
            {
                // ایجاد سبد خرید جدید برای کاربر
                userBasket = await CreateBasketForUserAsync(userBuyerId);
            }


            // انتقال آیتم‌ها از سبد ناشناس به سبد کاربر
            foreach (var anonymousItem in anonymousBasket.Items)
            {
                userBasket.AddItem(
                    anonymousItem.CatalogItemId,
                    anonymousItem.Quantity,
                    anonymousItem.UnitPrice
                );
            }

            // حذف سبد خرید ناشناس
            _context.Baskets.Remove(anonymousBasket);

            await _context.SaveChangesAsync();
        }

        public async Task ClearBasketAsync(int basketId)
        {
            var basket = await _context.Baskets
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == basketId);

            if (basket == null)
                throw new ArgumentException($"رکورد سبد خرید با کد {basketId} در سیستم موجود نمی‌باشد", nameof(basketId));
            // پاک کردن فیزیکی آیتم های سبد خرید در دیتابیس 
            await RemoveItemsFromBasketAsync(basket.Items.Select(o => o.Id).ToList());

            // پاک کردن لیست در حافظه
            basket.Clear();
        }

        public async Task<int> GetBasketItemsCountAsync(string buyerId)
        {
            var basket = await GetBasketByBuyerIdAsync(buyerId);
            return basket?.GetTotalItemsCount() ?? 0;
        }

        // متدهای کمکی خصوصی
        private async Task<Domain.Entities.Basket.Basket> GetBasketByBuyerIdAsync(string buyerId)
        {

            var basket = await _context.Baskets
                .Include(b => b.Items)
                    .ThenInclude(i => i.CatalogItem)
                        .ThenInclude(ci => ci.CatalogBrand)
                .Include(b => b.Items)
                    .ThenInclude(i => i.CatalogItem)
                        .ThenInclude(ci => ci.CatalogType)
                .Include(b => b.Items)
                    .ThenInclude(i => i.CatalogItem)
                        .ThenInclude(ci => ci.Images.OrderBy(img => img.SortOrder))
                .Include(b => b.Discount) // New code: include discount
                .FirstOrDefaultAsync(b => b.BuyerId == buyerId);


            if (basket != null)
            {
                // فیلتر آیتم‌هایی که IsRemoved = false
                var activeItems = basket.Items
                    .Where(item => !item.IsRemoved)
                    .ToList();

                // نرمال‌سازی تصاویر برای هر آیتم کاتالوگ
                foreach (var item in activeItems)
                    if (item.CatalogItem?.Images.Count > 0)
                        foreach (var image in item.CatalogItem.Images)
                            image.Src = _imageUrlService.Normalize(image.Src, ImageType.Product);
                
                // جایگزینی لیست داخلی _items با آیتم‌های فعال
                var itemsField = typeof(Domain.Entities.Basket.Basket)
                    .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);

                itemsField?.SetValue(basket, activeItems);
            }

            return basket;
        }
           

        private async Task<Domain.Entities.Basket.Basket> CreateBasketForUserAsync(string buyerId)
        {
            var basket = new Domain.Entities.Basket.Basket(buyerId);
            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();
            return basket;
        }

        private async Task<BasketDto> MapBasketToDtoAsync(Domain.Entities.Basket.Basket basket)
        {
            var basketDto = _mapper.Map<BasketDto>(basket);
            basketDto.DiscountAmount = basket.DiscountAmount;

            // مپ کردن آیتم‌ها با اطلاعات کامل
            foreach (var itemDto in basketDto.Items)
            {
                var basketItem = basket.Items.First(i => i.Id == itemDto.Id);
                if (!basketItem.IsRemoved)
                    await MapBasketItemToDtoAsync(basketItem, itemDto);
            }

            return basketDto;
        }

        private async Task MapBasketItemToDtoAsync(BasketItem basketItem, BasketItemDto itemDto)
        {
            var catalogItem = basketItem.CatalogItem;

            itemDto.CatalogItemName = catalogItem.Name;
            itemDto.BrandName = catalogItem.CatalogBrand?.Brand ?? string.Empty;
            itemDto.CategoryName = catalogItem.CatalogType?.Type ?? string.Empty;
            itemDto.AvailableStock = catalogItem.StockQuantity;

            // تنظیم آدرس تصویر
            var firstImage = catalogItem.Images?.FirstOrDefault();
            if (firstImage != null)
            {
                itemDto.CatalogItemImageUrl =_imageUrlService.Normalize(firstImage.Src,ImageType.Product);
            }
        }

        public async Task<MiniBasketDto> GetMiniBasketAsync(string buyerId)
        {
            var basket = await GetBasketByBuyerIdAsync(buyerId);

            if (basket == null)
            {
                return new MiniBasketDto();
            }

            var miniBasket = new MiniBasketDto
            {
                ItemsCount = basket.GetTotalItemsCount(),
                TotalPrice = basket.GetTotalPrice(),
                Items = basket.Items.Take(3).Select(item => new MiniBasketItemDto
                {
                    Id = item.Id,
                    CatalogItemId = item.CatalogItemId,
                    CatalogItemName = item.CatalogItem?.Name ?? string.Empty,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            // تنظیم آدرس تصاویر
            foreach (var item in miniBasket.Items)
            {
                var basketItem = basket.Items.First(i => i.Id == item.Id);
                var firstImage = basketItem.CatalogItem?.Images?.FirstOrDefault();
                if (firstImage != null)
                {
                    //  item.CatalogItemImageUrl = _imageUrlService.GetFullImageUrl(firstImage.Src);
                }
            }
            return miniBasket;
        }
    }
}