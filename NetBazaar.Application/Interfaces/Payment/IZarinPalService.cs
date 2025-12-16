using NetBazaar.Application.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Payment
{
    public interface IZarinPalService
    {
        Task<ZarinPalPaymentResponse> CreatePaymentRequestAsync(ZarinPalPaymentRequest request);
        Task<PaymentVerificationDto> VerifyPaymentAsync(string authority, int amount);
    }
    public class ZarinPalPaymentRequest
    {
        public string MerchantId { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string CallbackUrl { get; set; } = string.Empty;
    }

    public class ZarinPalPaymentResponse
    {
        public int Status { get; set; }
        public string Authority { get; set; } = string.Empty;
    }

}
