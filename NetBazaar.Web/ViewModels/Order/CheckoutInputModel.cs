using NetBazaar.Application.DTOs.Basket;
using NetBazaar.Application.DTOs.User;
using NetBazaar.Domain.Enums;

namespace NetBazaar.Web.EndPoint.ViewModels.Order
{
    public class CheckoutInputModel
    {
        public int SelectedAddressId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
