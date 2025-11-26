using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Catalog
{
    public class AddCatalogItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long Price { get; set; }
        public int CatalogTypeId { get; set; }
        public int BrandId { get; set; }
        public int StockQuantity { get; set; }
        public int ReorderThreshold { get; set; }
        public int MaxStockThreshold { get; set; }
        public List<CatalogItemFeatureDto> Features { get; set; } = new();
        public List<CatalogItemImageDto> Images { get; set; } = new();
    }
}
