using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetBazaar.Application.DTOs.Order;
using NetBazaar.Application.Interfaces.Order;
using NetBazaar.Domain.Entities.Basket;
using NetBazaar.Domain.Entities.Catalog;
using NetBazaar.Domain.Entities.Orders;
using NetBazaar.Domain.Entities.Users;
using NetBazaar.Domain.Enums;
using NetBazaar.Domain.ValueObjects;
using NetBazaar.Infrastructure.Services.Catalog;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetBazaar.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly INetBazaarDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageUrlService _imageUrlService;

        public OrderService(INetBazaarDbContext context, IMapper mapper, IImageUrlService imageUrlService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _imageUrlService = imageUrlService ?? throw new ArgumentNullException(nameof(imageUrlService));
        }

        public async Task<int> CreateOrderAsync(string buyerId, int addressId, PaymentMethod paymentMethod)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var basket = await GetUserBasketWithItemsAsync(buyerId);
                ValidateBasket(basket);

                var userAddress = await GetUserAddressAsync(addressId);
                var address = MapUserAddressToValueObject(userAddress);

                var order = await CreateOrderFromBasketAsync(buyerId, address, paymentMethod, basket);
                await ClearUserBasketAsync(buyerId, basket.Id);

                await transaction.CommitAsync();
                return order.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OrderDto>> GetOrdersForUserAsync(string buyerId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.BuyerId == buyerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId, string buyerId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.BuyerId == buyerId);

            return _mapper.Map<OrderDto?>(order);
        }

        #region Private Methods

        private async Task<Basket> GetUserBasketWithItemsAsync(string buyerId)
        {
            return await _context.Baskets
                .Include(b => b.Items)
                    .ThenInclude(i => i.CatalogItem)
                        .ThenInclude(ci => ci.Images)
                .FirstOrDefaultAsync(b => b.BuyerId == buyerId);
        }

        private static void ValidateBasket(Basket basket)
        {
            if (basket == null || !basket.Items.Any())
            {
                throw new InvalidOperationException("سبد خرید خالی است.");
            }
        }

        private async Task<UserAddress> GetUserAddressAsync(int addressId)
        {
            var userAddress = await _context.UserAddresses.FindAsync(addressId);

            if (userAddress == null)
            {
                throw new KeyNotFoundException($"آدرس با شناسه {addressId} یافت نشد.");
            }

            return userAddress;
        }

        private static Address MapUserAddressToValueObject(UserAddress userAddress)
        {
            return new Address(
                userAddress.PostalCode,
                userAddress.City,
                userAddress.ReceiverName,
                userAddress.AddressText,
                userAddress.PhoneNumber);
        }

        private async Task<Order> CreateOrderFromBasketAsync(
            string buyerId,
            Address address,
            PaymentMethod paymentMethod,
            Basket basket)
        {
            var order = new Order(buyerId, address, paymentMethod,  discountAmount: basket.DiscountAmount,
                discountId: basket.DiscountId,
                discount: basket.Discount);

            foreach (var basketItem in basket.Items)
            {
                var orderItem = CreateOrderItemFromBasketItem(basketItem);
                order.AddOrderItem(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private OrderItem CreateOrderItemFromBasketItem(BasketItem basketItem)
        {
            var catalogItem = basketItem.CatalogItem;
            var imagePath = GetProductImagePath(catalogItem);

            return new OrderItem(
                catalogItemId: basketItem.CatalogItemId,
                productName: catalogItem.Name,
                pictureUri: imagePath,
                unitPrice: basketItem.UnitPrice,
                units: basketItem.Quantity);
        }

        private string GetProductImagePath(CatalogItem catalogItem)
        {
            return _imageUrlService.Normalize(catalogItem?.Images?.FirstOrDefault()?.Src, ImageType.Product);
        }

        private async Task ClearUserBasketAsync(string buyerId, int basketId)
        {
            // Delete basket items first to maintain referential integrity
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM BasketItems WHERE BasketId = {0}",
                basketId);

            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Baskets WHERE Id = {0}",
                basketId);
        }

        #endregion
    }
}