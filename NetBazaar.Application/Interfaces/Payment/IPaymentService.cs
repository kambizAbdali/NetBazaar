using NetBazaar.Application.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Payment
{
    public interface IPaymentService
    {
        Task<PaymentDto> CreatePaymentAsync(int orderId);
        Task<PaymentDto> GetPaymentAsync(Guid paymentId);
        Task<bool> VerifyPaymentAsync(Guid paymentId, string authority);
    }
}