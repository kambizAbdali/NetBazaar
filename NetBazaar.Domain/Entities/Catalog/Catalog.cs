using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.Catalog
{
    public class CatalogItem
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty; // نام محصول
        public string Description { get; set; } = string.Empty; // توضیحات محصول
        public long Price { get; set; } // قیمت (به ریال)
        public int StockQuantity { get; set; } // موجودی انبار
        public int ReorderThreshold { get; set; } // حداقل موجودی برای هشدار
        public int MaxStockThreshold { get; set; } // حداکثر ظرفیت انبار

        // روابط
        public int CatalogTypeId { get; set; }
        public CatalogType CatalogType { get; set; }

        public int CatalogBrandId { get; set; }
        public CatalogBrand CatalogBrand { get; set; }

        public ICollection<CatalogItemFeature> Features { get; set; } = new List<CatalogItemFeature>();
        public ICollection<CatalogItemImage> Images { get; set; } = new List<CatalogItemImage>();
    }

}
