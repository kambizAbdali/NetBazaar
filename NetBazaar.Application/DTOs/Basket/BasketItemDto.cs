using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Basket
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public long CatalogItemId { get; set; }
        public string CatalogItemName { get; set; } = string.Empty;
        public string CatalogItemImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
        public string BrandName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int AvailableStock { get; set; }
    }
}
