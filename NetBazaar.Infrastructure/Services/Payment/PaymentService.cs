using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.DTOs.Payment;
using NetBazaar.Application.Interfaces.Payment;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NetBazaar.Domain.Entities.Payments;
namespace NetBazaar.Infrastructure.Services.Payment
{
        public class PaymentService : IPaymentService
        {
            private readonly INetBazaarDbContext _context;
            private readonly IZarinPalService _zarinPal;

            public PaymentService(INetBazaarDbContext context, IZarinPalService zarinPal)
            {
                _context = context;
                _zarinPal = zarinPal;
            }

            public async Task<PaymentDto> CreatePaymentAsync(int orderId)
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                    throw new InvalidOperationException("سفارش یافت نشد");

                var payment = new NetBazaar.Domain.Entities.Payments.Payment(orderId, order.GetTotalAmount());
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                return new PaymentDto
                {
                    PaymentId = payment.Id,
                    OrderId = order.Id,
                    Amount = payment.Amount,
                    BuyerId = order.BuyerId,
                    Description = $"پرداخت سفارش #{order.Id}",
                    Status = payment.Status.ToString()
                };
            }

            public async Task<PaymentDto> GetPaymentAsync(Guid paymentId)
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
                if (payment == null) throw new InvalidOperationException("پرداخت یافت نشد");

                var order = await _context.Orders.FirstAsync(o => o.Id == payment.OrderId);

                return new PaymentDto
                {
                    PaymentId = payment.Id,
                    OrderId = payment.OrderId,
                    Amount = payment.Amount,
                    BuyerId = order.BuyerId,
                    Authority = payment.Authority,
                    RefId = payment.RefId,
                    Description = $"پرداخت سفارش #{order.Id}",
                    Status = payment.Status.ToString()
                };
            }

            public async Task<bool> VerifyPaymentAsync(Guid paymentId, string authority)
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
                if (payment == null) throw new InvalidOperationException("پرداخت یافت نشد");

                var order = await _context.Orders.Include(o => o.OrderItems)
                                                 .FirstAsync(o => o.Id == payment.OrderId);

                var verification = await _zarinPal.VerifyPaymentAsync(authority, (int)payment.Amount);

                if (verification.Status == 100) // موفق
                {
                    payment.MarkAsPaid(authority, verification.RefId.ToString());
                    order.MarkAsPaid();

                    _context.Payments.Update(payment);
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
        }
    
}
