namespace NetBazaar.Web.EndPoint.ViewModels.Basket
{
    public class MiniBasketViewModel
    {
        public int ItemsCount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<MiniBasketItemViewModel> Items { get; set; } = new();
        public bool IsEmpty => ItemsCount == 0;

        // برای نمایش زیبا
        public string DisplayTotalPrice => TotalPrice.ToString("N0") + " تومان";
        public string DisplayItemsCount => ItemsCount.ToString("N0");
    }

    public class MiniBasketItemViewModel
    {
        public int Id { get; set; }
        public long CatalogItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // برای نمایش زیبا
        public string DisplayUnitPrice => UnitPrice.ToString("N0") + " تومان";
        public string DisplayTotalPrice => TotalPrice.ToString("N0") + " تومان";
        public string ShortName => Name.Length > 30 ? Name.Substring(0, 30) + "..." : Name;
    }
}