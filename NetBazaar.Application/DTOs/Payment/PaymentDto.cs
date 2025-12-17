using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Payment
{
    public class PaymentDto
    {
        public Guid PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public string? UserMobile { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Authority { get; set; }
        public string? RefId { get; set; }
        public string Status { get; set; } = "Pending";
    }
    public class PaymentVerificationDto
    {
        public int Status { get; set; }
        public long RefId { get; set; }
    }
}