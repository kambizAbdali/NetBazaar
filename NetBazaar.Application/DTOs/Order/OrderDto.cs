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
        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    }
}
