
using NetBazaar.Domain.Common;
using NetBazaar.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;

namespace NetBazaar.Domain.Discounts
{
    public class Discount : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // نوع تخفیف: درصدی یا مبلغی
        public bool UsePercentage { get; set; }
        public int? DiscountPercentage { get; set; }
        public int? DiscountAmount { get; set; }

        // بازه زمانی تخفیف
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // مدیریت کد تخفیف
        public bool RequiresCouponCode { get; set; }
        public string CouponCode { get; set; } = string.Empty;

        // نوع تخفیف (برای چه گروهی اعمال شود)
        public DiscountType DiscountType { get; set; }
        public int DiscountTypeId { get; set; }

        // محدودیت استفاده
        public DiscountLimitationType LimitationType { get; set; }
        public int? LimitationTimes { get; set; }

        // ارتباطات (اختیاری)
        public ICollection<CatalogItem>? CatalogItems { get; set; }
    }
}
