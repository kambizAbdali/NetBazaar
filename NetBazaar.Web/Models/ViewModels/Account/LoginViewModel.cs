using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetBazaar.Web.EndPoint.Models.ViewModels.Account
{
    public class LoginViewModel
    {
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

        [DisplayName("RememberMe")]
        public bool IsPersistent { get; set; } = false;
        public string ReturnUrl { get; set; } = "/";
    }
}
