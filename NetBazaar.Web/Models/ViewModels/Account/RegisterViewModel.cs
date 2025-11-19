namespace NetBazaar.Web.EndPoint.Models.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "نام اجباری است.")]
        [StringLength(100, ErrorMessage = "نام نمی‌تواند بیش از 100 کاراکتر باشد.")]
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Required(ErrorMessage = "نام خانوادگی اجباری است.")]
        [StringLength(100, ErrorMessage = "نام خانوادگی نمی‌تواند بیش از 100 کاراکتر باشد.")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "ایمیل اجباری است.")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل معتبر نیست.")]
        [StringLength(256, ErrorMessage = "ایمیل نمی‌تواند بیش از 256 کاراکتر باشد.")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "رمز عبور اجباری است.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید بین 6 تا 100 کاراکتر باشد.")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تایید رمز عبور اجباری است.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمز عبور و تأیید آن هم‌خوانی ندارند.")]
        [Display(Name ="تکرار رمز عبور")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "شماره همراه اجباری است.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "شماره همراه")]
        public string PhoneNumber { get; set; }
    }
}
