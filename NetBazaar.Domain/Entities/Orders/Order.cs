using NetBazaar.Domain.Enums;
using NetBazaar.Domain.ValueObjects;
using System.Net;

namespace NetBazaar.Domain.Entities.Orders
{
    public class Order
    {
        private readonly List<OrderItem> _orderItems = new();

        private Order() { } // For EF Core

        public Order(string buyerId, Address address, PaymentMethod paymentMethod)
        {
            BuyerId = buyerId;
            Address = address;
            PaymentMethod = paymentMethod;
            PaymentStatus = PaymentStatus.Pending;
            OrderStatus = OrderStatus.Processing;
            OrderDate = DateTime.Now;
        }

        public int Id { get; private set; }
        public string BuyerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public Address Address { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        // زمان‌های وضعیت
        public DateTime? PaidDate { get; private set; }
        public DateTime? CancellationDate { get; private set; }
        public DateTime? DeliveryDate { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public void AddOrderItem(OrderItem orderItem) 
        {
            _orderItems.Add(orderItem);
        }
        public void AddAddress( Address address)
        {
            Address = address;
        }

        public void MarkAsPaid()
        {
            PaymentStatus = PaymentStatus.Paid;
            PaidDate = DateTime.Now;
        }

        public void CancelOrder()
        {
            OrderStatus = OrderStatus.Cancelled;
            CancellationDate = DateTime.Now;
        }

        public void MarkAsDelivered()
        {
            OrderStatus = OrderStatus.Delivered;
            DeliveryDate = DateTime.Now;
        }

        // مجموع مبلغ سفارش (UnitPrice × Units برای همه آیتم‌ها)
        public decimal GetTotalAmount() => _orderItems.Sum(i => i.UnitPrice * i.Units);
    }
}
