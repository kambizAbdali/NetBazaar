using NetBazaar.Application.DTOs.Order;
using NetBazaar.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Order
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(string buyerId, int addressId, PaymentMethod paymentMethod);
        Task<OrderDto?> GetOrderByIdAsync(int orderId, string buyerId);
        Task<List<OrderDto>> GetOrdersForUserAsync(string buyerId);
    }
}
