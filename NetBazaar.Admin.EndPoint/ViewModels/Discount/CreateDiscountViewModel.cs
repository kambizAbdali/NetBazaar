using Microsoft.AspNetCore.Mvc.Rendering;
using NetBazaar.Domain.Discounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetBazaar.Admin.EndPoint.ViewModels.Discounts
{
    public class CreateDiscountViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "نام تخفیف الزامی است.")]
        [StringLength(100, ErrorMessage = "نام تخفیف نباید بیشتر از 100 کاراکتر باشد.")]
        public string Name { get; set; } = string.Empty;

        public bool UsePercentage { get; set; } = true;

        [Range(1, 100, ErrorMessage = "درصد تخفیف باید بین 1 تا 100 باشد.")]
        public int? DiscountPercentage { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "مبلغ تخفیف باید بزرگتر از صفر باشد.")]
        public int? DiscountAmount { get; set; }

        [Required(ErrorMessage = "تاریخ شروع الزامی است.")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "تاریخ پایان الزامی است.")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(7);

        public bool RequiresCouponCode { get; set; } = false;

        [StringLength(50, ErrorMessage = "کد کوپن نباید بیشتر از 50 کاراکتر باشد.")]
        public string? CouponCode { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; } = DiscountType.AssignedToProducts;

        [Required]
        public DiscountLimitationType LimitationType { get; set; } = DiscountLimitationType.Unlimited;

        [Range(1, int.MaxValue, ErrorMessage = "تعداد محدودیت باید بزرگتر از صفر باشد.")]
        public int? LimitationTimes { get; set; }

        // لیست محصولات برای نمایش در dropdown
        public List<SelectListItem>? CatalogItems { get; set; }

        // آی‌دی‌های محصولات انتخاب شده
        public List<long>? SelectedCatalogItemIds { get; set; }

        // برای نگهداری نام‌های محصولات انتخاب شده (فقط نمایش)
        public List<string>? SelectedCatalogItemNames { get; set; }

        // رشته آی‌دی‌ها با کاما (برای سازگاری با کد قدیمی)
        public string? CatalogItemIdsRaw
        {
            get
            {
                if (SelectedCatalogItemIds == null || !SelectedCatalogItemIds.Any())
                    return null;
                return string.Join(",", SelectedCatalogItemIds);
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    SelectedCatalogItemIds = value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => long.TryParse(s.Trim(), out var id) ? id : 0)
                        .Where(id => id > 0)
                        .ToList();
                }
            }
        }

        // قوانین سفارشی
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult("تاریخ پایان باید بعد از تاریخ شروع باشد.", new[] { nameof(EndDate) });
            }

            if (UsePercentage)
            {
                if (!DiscountPercentage.HasValue)
                    yield return new ValidationResult("در حالت درصدی، مقدار درصد تخفیف الزامی است.", new[] { nameof(DiscountPercentage) });
            }
            else
            {
                if (!DiscountAmount.HasValue)
                    yield return new ValidationResult("در حالت مبلغی، مقدار مبلغ تخفیف الزامی است.", new[] { nameof(DiscountAmount) });
            }

            if (RequiresCouponCode && string.IsNullOrWhiteSpace(CouponCode))
            {
                yield return new ValidationResult("در صورت نیاز به کوپن، وارد کردن کد کوپن الزامی است.", new[] { nameof(CouponCode) });
            }

            if (!RequiresCouponCode)
            {
                CouponCode = string.Empty;
            }

            if (LimitationType == DiscountLimitationType.NTimesOnly && !LimitationTimes.HasValue)
            {
                yield return new ValidationResult("در حالت محدودیت تعداد، وارد کردن تعداد الزامی است.", new[] { nameof(LimitationTimes) });
            }

            // اعتبارسنجی اختیاری بودن محصولات (اگر نیاز باشد)
            // اگر انتخاب محصول الزامی است، خط زیر را فعال کنید:
            // if (SelectedCatalogItemIds == null || !SelectedCatalogItemIds.Any())
            // {
            //     yield return new ValidationResult("حداقل یک محصول باید انتخاب شود.", new[] { nameof(SelectedCatalogItemIds) });
            // }
        }
    }
}