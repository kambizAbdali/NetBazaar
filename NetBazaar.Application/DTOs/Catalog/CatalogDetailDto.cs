using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Catalog
{
    public class CatalogDetailDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long Price { get; set; }
        public int? DiscountPercent { get; set; }
        public List<string> Images { get; set; } = new();
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public IEnumerable<string> Features { get; set; } = Enumerable.Empty<string>();
    }
}

