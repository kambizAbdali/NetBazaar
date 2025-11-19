using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NetBazaar.Web.EndPoint.ViewModels
{
    public class CartViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public DiscountViewModel AppliedDiscount { get; set; }

        // Computed Properties
        public int TotalItems => Items.Sum(x => x.Quantity);
        public decimal TotalPrice => Items.Sum(x => x.TotalPrice);
        public decimal DiscountAmount => AppliedDiscount?.CalculateDiscount(TotalPrice) ?? 0;
        public decimal FinalPrice => TotalPrice - DiscountAmount;
        public decimal TaxAmount => FinalPrice * 0.09m; // 9% tax
        public decimal GrandTotal => FinalPrice + TaxAmount;

        public bool IsEmpty => !Items.Any();
        public bool HasDiscount => AppliedDiscount != null;

        // Shipping
        public decimal ShippingCost { get; set; }
        public string ShippingMethod { get; set; }

        // Validation
        public bool IsValid => Items.All(x => x.IsAvailable && x.IsActive);
        public List<string> ValidationErrors
        {
            get
            {
                var errors = new List<string>();

                foreach (var item in Items.Where(x => !x.IsAvailable))
                {
                    errors.Add($"محصول '{item.ProductName}' در حال حاضر موجود نیست");
                }

                foreach (var item in Items.Where(x => !x.IsActive))
                {
                    errors.Add($"محصول '{item.ProductName}' غیرفعال شده است");
                }

                foreach (var item in Items.Where(x => x.Quantity > x.MaxStock))
                {
                    errors.Add($"تعداد درخواستی برای محصول '{item.ProductName}' بیشتر از موجودی است. حداکثر {item.MaxStock} عدد موجود است");
                }


                return errors;
            }
        }
    }

    public class CartItemViewModel
    {
        public string Id { get; set; }
        public int ProductId { get; set; }

        [Display(Name = "نام محصول")]
        public string ProductName { get; set; }

        [Display(Name = "دسته بندی")]
        public string CategoryName { get; set; }

        [Display(Name = "قیمت واحد")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [Display(Name = "تعداد")]
        [Range(1, 10, ErrorMessage = "تعداد باید بین 1 تا 10 باشد")]
        public int Quantity { get; set; }

        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }

        [Display(Name = "اسلاگ")]
        public string ProductSlug { get; set; }

        // Stock Information
        [Display(Name = "موجودی")]
        public int StockQuantity { get; set; }

        [Display(Name = "حداکثر تعداد قابل سفارش")]
        public int MaxStock => Math.Min(StockQuantity, 10);

        [Display(Name = "در دسترس")]
        public bool IsAvailable => StockQuantity > 0;

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        // Discount Information
        public bool HasDiscount { get; set; }
        public decimal? DiscountedUnitPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }

        // Computed Properties
        public decimal ActualUnitPrice => DiscountedUnitPrice ?? UnitPrice;
        public decimal TotalPrice => ActualUnitPrice * Quantity;
        public decimal OriginalTotalPrice => UnitPrice * Quantity;
        public decimal ItemDiscount => OriginalTotalPrice - TotalPrice;

        // Validation
        public bool CanAddMore => Quantity < MaxStock;
        public bool ShouldShowWarning => Quantity >= MaxStock * 0.8m; // Show warning when接近حداکثر

        // Product Properties for Validation
        public bool ProductExists { get; set; } = true;
        public bool ProductIsActive { get; set; } = true;
    }

    public class DiscountViewModel
    {
        public string Id { get; set; }

        [Display(Name = "کد تخفیف")]
        public string Code { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "نوع تخفیف")]
        public DiscountType Type { get; set; }

        [Display(Name = "مقدار تخفیف")]
        public decimal Value { get; set; }

        [Display(Name = "حداقل مقدار سفارش")]
        public decimal MinimumOrderAmount { get; set; }

        [Display(Name = "حداکثر مقدار تخفیف")]
        public decimal? MaximumDiscountAmount { get; set; }

        [Display(Name = "تعداد استفاده")]
        public int UsageCount { get; set; }

        [Display(Name = "حداکثر استفاده")]
        public int? UsageLimit { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ پایان")]
        public DateTime EndDate { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "قابل استفاده")]
        public bool IsApplicable { get; set; }

        [Display(Name = "پیغام")]
        public string Message { get; set; }

        // Product restrictions
        public List<int> ApplicableProductIds { get; set; } = new List<int>();
        public List<int> ApplicableCategoryIds { get; set; } = new List<int>();

        // User restrictions
        public bool IsFirstOrderOnly { get; set; }
        public List<string> ApplicableUserIds { get; set; } = new List<string>();

        // Computed Properties
        public bool IsValid => IsActive &&
                              DateTime.Now >= StartDate &&
                              DateTime.Now <= EndDate &&
                              (!UsageLimit.HasValue || UsageCount < UsageLimit.Value);

        public bool HasUsageLimit => UsageLimit.HasValue;
        public int RemainingUsage => UsageLimit.HasValue ? UsageLimit.Value - UsageCount : int.MaxValue;
        public bool IsExpired => DateTime.Now > EndDate;
        public bool IsNotStarted => DateTime.Now < StartDate;

        public decimal CalculateDiscount(decimal orderAmount)
        {
            if (!IsValid || orderAmount < MinimumOrderAmount)
                return 0;

            decimal discount = 0;

            switch (Type)
            {
                case DiscountType.Percentage:
                    discount = orderAmount * (Value / 100);
                    break;
                case DiscountType.FixedAmount:
                    discount = Value;
                    break;
                case DiscountType.FreeShipping:
                    // This would be handled separately in shipping calculation
                    discount = 0;
                    break;
            }

            // Apply maximum discount limit
            if (MaximumDiscountAmount.HasValue && discount > MaximumDiscountAmount.Value)
            {
                discount = MaximumDiscountAmount.Value;
            }

            return discount;
        }

        public DiscountValidationResult ValidateForCart(CartViewModel cart)
        {
            var result = new DiscountValidationResult { IsValid = true };

            if (!IsActive)
            {
                result.IsValid = false;
                result.Message = "کد تخفیف معتبر نیست";
                return result;
            }

            if (IsExpired)
            {
                result.IsValid = false;
                result.Message = "کد تخفیف منقضی شده است";
                return result;
            }

            if (IsNotStarted)
            {
                result.IsValid = false;
                result.Message = "کد تخفیف هنوز فعال نشده است";
                return result;
            }

            if (HasUsageLimit && UsageCount >= UsageLimit.Value)
            {
                result.IsValid = false;
                result.Message = "محدودیت استفاده از کد تخفیف به پایان رسیده است";
                return result;
            }

            if (cart.TotalPrice < MinimumOrderAmount)
            {
                result.IsValid = false;
                result.Message = $"حداقل مقدار سفارش برای استفاده از این کد تخفیف {MinimumOrderAmount.ToString("N0")} تومان می باشد";
                return result;
            }

            // Check product restrictions
            if (ApplicableProductIds.Any() &&
                !cart.Items.Any(item => ApplicableProductIds.Contains(item.ProductId)))
            {
                result.IsValid = false;
                result.Message = "این کد تخفیف برای محصولات انتخابی شما قابل استفاده نیست";
                return result;
            }

            // Check category restrictions
            if (ApplicableCategoryIds.Any() &&
                !cart.Items.Any(item => ApplicableCategoryIds.Contains(int.Parse(item.Id))))
            {
                result.IsValid = false;
                result.Message = "این کد تخفیف برای دسته بندی های انتخابی شما قابل استفاده نیست";
                return result;
            }

            result.DiscountAmount = CalculateDiscount(cart.TotalPrice);
            result.Message = "کد تخفیف با موفقیت اعمال شد";

            return result;
        }
    }

    public class DiscountValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public enum DiscountType
    {
        [Display(Name = "درصدی")]
        Percentage = 0,

        [Display(Name = "مبلغ ثابت")]
        FixedAmount = 1,

        [Display(Name = "ارسال رایگان")]
        FreeShipping = 2
    }

    // Additional ViewModels for Cart Operations
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public string CartId { get; set; }
    }

    public class AddToCartResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CartViewModel Cart { get; set; }
        public int CartItemCount { get; set; }
        public decimal CartTotal { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public string ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class RemoveCartItemRequest
    {
        public string ItemId { get; set; }
    }

    public class ApplyDiscountRequest
    {
        public string DiscountCode { get; set; }
        public string CartId { get; set; }
    }

    public class ApplyDiscountResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }
        public DiscountViewModel Discount { get; set; }
    }

}