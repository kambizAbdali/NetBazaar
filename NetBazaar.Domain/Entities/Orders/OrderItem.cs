namespace NetBazaar.Domain.Entities.Orders
{
    public class OrderItem
    {
        private OrderItem() { } // For EF Core

        public OrderItem(long catalogItemId, string productName, string pictureUri, decimal unitPrice, int units)
        {
            CatalogItemId = catalogItemId;
            ProductName = productName;
            PictureUri = pictureUri;
            UnitPrice = unitPrice;
            Units = units;
        }

        public long Id { get; private set; }
        public long CatalogItemId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUri { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Units { get; private set; }
    }
}
