namespace NetBazaar.Application.DTOs.Order
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int CatalogItemId { get; set; }
        public string ProductName { get; set; }
        public string PictureUri { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
        public decimal TotalPrice => UnitPrice * Units;
    }
}
