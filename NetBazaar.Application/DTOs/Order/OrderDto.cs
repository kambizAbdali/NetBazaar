namespace NetBazaar.Application.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string AddressText { get; set; }
        public string ReceiverName { get; private set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodKey { get; set; }
        public string PaymenStatus { get; set; }
        public string PaymentStatusKey { get; set; }= string.Empty;
        public string OrderStatus { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();

        // New code: اضافه کردن فیلدهای تخفیف
        public decimal DiscountAmount { get; set; }
        public string? DiscountCouponCode { get; set; }

        // New code: محاسبه قیمت‌ها با تخفیف
        public decimal TotalPriceWithoutDiscount => Items.Sum(i => i.TotalPrice);
        public decimal TotalPrice => TotalPriceWithoutDiscount - DiscountAmount;
    }
}
