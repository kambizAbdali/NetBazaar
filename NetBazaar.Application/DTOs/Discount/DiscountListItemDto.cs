using NetBazaar.Domain.Discounts;
using NetBazaar.Domain.Extensions;
using System;

namespace NetBazaar.Application.DTOs.Discounts
{
    public class DiscountListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool UsePercentage { get; set; }
        public int? DiscountPercentage { get; set; }
        public int? DiscountAmount { get; set; }
        public string CouponCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool RequiresCouponCode { get; set; }
        public string DiscountTypeString { get; set; }
        public string LimitationTypeString { get; set; }
    }
}
