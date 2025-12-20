using System.ComponentModel.DataAnnotations;

namespace NetBazaar.Domain.Discounts
{
    public enum DiscountType
    {
        [Display(Name = "تخفیف برای محصولات")]
        AssignedToProducts = 1,

        [Display(Name = "تخفیف برای دسته‌بندی")]
        AssignedToCategories = 2,

        [Display(Name = "تخفیف برای مشتری")]
        AssignedToCustomers = 3,

        [Display(Name = "تخفیف برای برند")]
        AssignedToBrands = 4
    }

    public enum DiscountLimitationType
    {
        [Display(Name = "بدون محدودیت")]
        Unlimited = 1,

        [Display(Name = "تنها N بار")]
        NTimesOnly = 2,

        [Display(Name = "N بار به ازای هر مشتری")]
        NTimesPerCustomer = 3
    }
}
