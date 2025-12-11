using NetBazaar.Application.DTOs.Basket;
using NetBazaar.Application.DTOs.User;
using NetBazaar.Domain.Enums;

namespace NetBazaar.Web.EndPoint.ViewModels.Order
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CheckoutViewModel
    {
        // داده‌های نمایشی
        [ValidateNever] // این پراپرتی فقط برای نمایش است، نیازی به اعتبارسنجی ندارد
        public BasketDto Basket { get; set; }

        [ValidateNever] // لیست آدرس‌ها فقط برای نمایش است
        public List<UserAddressDto> Addresses { get; set; }

        // داده‌های ورودی کاربر
        [Required(ErrorMessage = "انتخاب آدرس الزامی است.")]
        [Range(1, int.MaxValue, ErrorMessage = "آدرس انتخابی معتبر نیست.")]
        public int SelectedAddressId { get; set; }

        [Required(ErrorMessage = "انتخاب روش پرداخت الزامی است.")]
        public PaymentMethod PaymentMethod { get; set; }
    }


}
