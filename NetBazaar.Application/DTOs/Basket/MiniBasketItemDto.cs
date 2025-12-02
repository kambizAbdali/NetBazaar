using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Basket
{
    public class MiniBasketItemDto
    {
        public int Id { get; set; }
        public long CatalogItemId { get; set; }
        public string CatalogItemName { get; set; } = string.Empty;
        public string CatalogItemImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
