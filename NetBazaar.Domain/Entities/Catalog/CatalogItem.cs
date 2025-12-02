namespace NetBazaar.Domain.Entities.Catalog
{
    public class CatalogItem
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;     // نام محصول
        public string Description { get; set; } = string.Empty; // توضیحات
        public long Price { get; set; }                      // قیمت (ریال)
        public int StockQuantity { get; set; }               // موجودی انبار
        public int ReorderThreshold { get; set; }            // حداقل موجودی برای هشدار
        public int MaxStockThreshold { get; set; }           // حداکثر ظرفیت

        // روابط
        public int CatalogTypeId { get; set; }
        public CatalogType CatalogType { get; set; }
        public int CatalogBrandId { get; set; }
        public CatalogBrand CatalogBrand { get; set; }

        public ICollection<CatalogItemFeature> Features { get; set; } = new List<CatalogItemFeature>();
        public ICollection<CatalogItemImage> Images { get; set; } = new List<CatalogItemImage>();

        // افزودنی‌ها (بدون حذف فیلدهای موجود)
        public int? DiscountPercent { get; set; }            // درصد تخفیف
        public DateTime? DiscountUntil { get; set; }         // اعتبار تخفیف
        public ICollection<CatalogItemRating> Ratings { get; set; } = new List<CatalogItemRating>();
        public ICollection<CatalogItemTag> Tags { get; set; } = new List<CatalogItemTag>();
    }


    public class CatalogItemRating
    {
        public long Id { get; set; }
        public long CatalogItemId { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public int Score { get; set; } // 1..5
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CatalogItemTag
    {
        public long Id { get; set; }
        public long CatalogItemId { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public string Value { get; set; } = string.Empty; // مثال: Featured, BestSeller
    }
}
