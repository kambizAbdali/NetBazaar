using NetBazaar.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.Payments
{
    public class Payment
    {
        private Payment() { } // EF

        public Guid Id { get; private set; }
        public int OrderId { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? PaidDate { get; private set; }
        public string? Authority { get; private set; }
        public string? RefId { get; private set; }

        public Payment(int orderId, decimal amount)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Amount = amount;
            Status = PaymentStatus.Pending;
            CreatedAt = DateTime.Now;
        }

        public void MarkAsPaid(string authority, string refId)
        {
            Status = PaymentStatus.Paid;
            Authority = authority;
            RefId = refId;
            PaidDate = DateTime.Now;
        }

        public void MarkAsPending() => Status = PaymentStatus.Pending;
    }
}
