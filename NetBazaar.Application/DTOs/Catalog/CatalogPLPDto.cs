namespace NetBazaar.Application.DTOs.Catalog
{
    public class CatalogPLPDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Price { get; set; }

        public string? FirstImageSrc { get; set; } // اولین تصویر
        public double Rating { get; set; }         // 0..5 میانگین
        public int ReviewCount { get; set; }       // تعداد رای‌ها
        public int? DiscountPercent { get; set; }  // 0..100
        public bool IsFeatured { get; set; }       // بر اساس Tag
        public int CatalogBrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public int CatalogTypeId { get; set; }
        public int StockQuantity { get; set; }
        public int ReorderThreshold { get; set; }  // برای تعیین وضعیت موجودی
    }
}
