using NetBazaar.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Catalog
{

    public class CatalogDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;          // نام محصول
        public string Description { get; set; } = string.Empty;   // توضیحات محصول
        public long Price { get; set; }                           // قیمت (ریال)
        public int StockQuantity { get; set; }                    // موجودی انبار
        public int ReorderThreshold { get; set; }                 // حداقل موجودی برای هشدار
        public int MaxStockThreshold { get; set; }                // حداکثر ظرفیت انبار

        // روابط
        public int CatalogTypeId { get; set; }
        public CatalogType CatalogType { get; set; }

        public int CatalogBrandId { get; set; }
        public CatalogBrand CatalogBrand { get; set; }

        // ویژگی‌ها و تصاویر
        public List<CatalogItemFeatureDto> Features { get; set; } = new();
        public List<CatalogItemImageDto> Images { get; set; } = new();
    }
}