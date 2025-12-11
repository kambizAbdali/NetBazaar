using FluentValidation;
using NetBazaar.Web.EndPoint.ViewModels.Order;

namespace NetBazaar.Web.EndPoint.Validators
{
    public class CheckoutViewModelValidator : AbstractValidator<CheckoutViewModel>
    {
        public CheckoutViewModelValidator()
        {
            RuleFor(x => x.SelectedAddressId)
                .GreaterThan(0).WithMessage("انتخاب آدرس الزامی است.");

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("روش پرداخت معتبر نیست.");

               /* RuleFor(x => x.Basket)
                .NotNull().WithMessage("سبد خرید نمی‌تواند خالی باشد.")
                .Must(b => b.Items.Any()).WithMessage("سبد خرید باید حداقل یک محصول داشته باشد.");*/
        }
    }

}
