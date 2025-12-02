namespace NetBazaar.Web.EndPoint.ViewModels.Basket
{
    public class BasketItemViewModel
    {
        public int Id { get; set; }
        public long CatalogItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int AvailableStock { get; set; }

        // برای نمایش زیبا
        public string DisplayUnitPrice => UnitPrice.ToString("N0") + " تومان";
        public string DisplayTotalPrice => TotalPrice.ToString("N0") + " تومان";
    }
}
