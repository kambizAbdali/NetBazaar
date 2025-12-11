using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    public class UserAddressDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "شهر الزامی است.")]
        [MaxLength(100, ErrorMessage = "نام شهر نباید بیشتر از 100 کاراکتر باشد.")]
        public string City { get; set; } = "";

        [Required(ErrorMessage = "نام گیرنده الزامی است.")]
        [MaxLength(100, ErrorMessage = "نام گیرنده نباید بیشتر از 100 کاراکتر باشد.")]
        public string ReceiverName { get; set; } = "";

        [Required(ErrorMessage = "شماره تلفن الزامی است.")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره تلفن باید معتبر باشد.")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "کد پستی الزامی است.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "کد پستی باید 10 رقم باشد.")]
        public string PostalCode { get; set; } = "";

        [Required(ErrorMessage = "آدرس الزامی است.")]
        [MaxLength(500, ErrorMessage = "آدرس نباید بیشتر از 500 کاراکتر باشد.")]
        public string AddressText { get; set; } = "";

        public bool IsDefault { get; set; }
    }
}
