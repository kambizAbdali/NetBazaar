namespace NetBazaar.Web.EndPoint.ViewModels.Basket
{
    public class BasketViewModel
    {
        public int Id { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public List<BasketItemViewModel> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public int TotalItems { get; set; }
        public bool IsEmpty { get; set; } = true;
        public bool HasItems => Items?.Any() == true;
    }
}
