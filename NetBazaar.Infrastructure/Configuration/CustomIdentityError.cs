using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace NetBazaar.Infrastructure.Identity
{
    public class CustomIdentityError : IdentityErrorDescriber
    {
        // خطاهای مربوط به نام کاربری
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"نام کاربری '{userName}' قبلاً انتخاب شده است. لطفاً نام کاربری دیگری انتخاب کنید."
            };
        }

        public override IdentityError InvalidUserName(string? userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = "نام کاربری فقط می‌تواند شامل حروف انگلیسی، اعداد و کاراکترهای - _ . باشد."
            };
        }

        // خطاهای مربوط به ایمیل
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"ایمیل '{email}' قبلاً در سیستم ثبت شده است."
            };
        }
        public override IdentityError InvalidEmail(string? email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = "فرمت ایمیل وارد شده معتبر نیست."
            };
        }
        // خطاهای مربوط به رمز عبور
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"رمز عبور باید حداقل {length} کاراکتر باشد."
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "رمز عبور باید حداقل شامل یک کاراکتر ویژه (!@#$% و ...) باشد."
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "رمز عبور باید حداقل شامل یک عدد (0-9) باشد."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "رمز عبور باید حداقل شامل یک حرف کوچک انگلیسی (a-z) باشد."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "رمز عبور باید حداقل شامل یک حرف بزرگ انگلیسی (A-Z) باشد."
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "رمز عبور فعلی نادرست است."
            };
        }
        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = $"کاربر در حال حاضر دارای نقش '{role}' می‌باشد."
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserNotInRole),
                Description = $"کاربر دارای نقش '{role}' نمی‌باشد."
            };
        }

        // خطاهای مربوط به توکن
        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = "کد تأیید نامعتبر یا منقضی شده است."
            };
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Code = nameof(RecoveryCodeRedemptionFailed),
                Description = "کد بازیابی نامعتبر است."
            };
        }

        // خطاهای عمومی
        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = "خطای ناشناخته‌ای رخ داده است."
            };
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = "عملیات به دلیل تداخل دسترسی ناموفق بود. لطفاً مجدداً تلاش کنید."
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = $"رمز عبور باید حداقل شامل {uniqueChars} کاراکتر متفاوت باشد."
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = "این حساب کاربری قبلاً به یک کاربر دیگر متصل شده است."
            };
        }
        // خطاهای مربوط به تأیید ایمیل/شماره تلفن
        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = "کاربر از قبل دارای رمز عبور می‌باشد."
            };
        }

        // خطاهای مربوط به نقش‌ها
        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = $"نقش '{role}' از قبل وجود دارد."
            };
        }

        public override IdentityError InvalidRoleName(string? role)
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = $"نام نقش '{role}' معتبر نیست."
            };
        }

        // خطاهای سفارشی اضافی
        public IdentityError UserNotFound()
        {
            return new IdentityError
            {
                Code = nameof(UserNotFound),
                Description = "کاربری با این مشخصات یافت نشد."
            };
        }

        public IdentityError OperationNotAllowed()
        {
            return new IdentityError
            {
                Code = nameof(OperationNotAllowed),
                Description = "این عملیات مجاز نمی‌باشد."
            };
        }
    }
}