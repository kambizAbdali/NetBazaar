//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace NetBazaar.Domain.Entities.Catalog
//{
//    public class CatalogItemRating
//    {
//        public long Id { get; set; }
//        public long CatalogItemId { get; set; }
//        public CatalogItem CatalogItem { get; set; }
//        public int Score { get; set; } // 1..5
//        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//    }

//    public class CatalogItemTag
//    {
//        public long Id { get; set; }
//        public long CatalogItemId { get; set; }
//        public CatalogItem CatalogItem { get; set; }
//        public string Value { get; set; } = string.Empty;
//    }

//    // اختیاری: فیلدهای تخفیف روی CatalogItem (افزودن، نه حذف)
//    public partial class CatalogItem
//    {
//        public int? DiscountPercent { get; set; } // 0..100
//        public DateTime? DiscountUntil { get; set; }
//    }

//}
