using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOrUpdateUserAddressDto
    {
        [Display(Name = "نام دریافت‌کننده")]
        [Required(ErrorMessage = "نام دریافت‌کننده الزامی است")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "نام باید بین 2 تا 100 کاراکتر باشد")]
        public string ReceiverName { get; set; } = "";

        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "شماره تماس الزامی است")]
        [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "فرمت شماره تماس صحیح نیست")]
        public string PhoneNumber { get; set; } = "";

        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = "کد پستی الزامی است")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "کد پستی باید 10 رقمی باشد")]
        public string PostalCode { get; set; } = "";

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "آدرس الزامی است")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "آدرس باید بین 10 تا 500 کاراکتر باشد")]
        public string AddressText { get; set; } = "";

        [Display(Name = "آدرس پیش‌فرض")]
        public bool IsDefault { get; set; }

        public string City { get; set; } = "";
    }
}