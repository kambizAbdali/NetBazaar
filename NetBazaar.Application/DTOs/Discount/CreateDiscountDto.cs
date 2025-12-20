using NetBazaar.Domain.Discounts;
using NetBazaar.Domain.Extensions;
using System;
using System.Collections.Generic;

namespace NetBazaar.Application.DTOs.Discounts
{
    public class CreateDiscountDto
    {
        public string Name { get; set; } = string.Empty;
        public bool UsePercentage { get; set; }
        public int? DiscountPercentage { get; set; }
        public int? DiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool RequiresCouponCode { get; set; }
        public string? CouponCode { get; set; }
        public DiscountType DiscountType { get; set; }
        public string DiscountTypeString => DiscountType.GetDisplayName();
        public DiscountLimitationType LimitationType { get; set; }
        public string LimitationTypeString => LimitationType.GetDisplayName();
        public int? LimitationTimes { get; set; }
        public List<long>? CatalogItemIds { get; set; }
    }
}
