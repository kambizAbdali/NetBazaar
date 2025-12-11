using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Enums
{
    public enum PaymentMethod
    {
        Online = 1,
        CashOnDelivery = 2
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2
    }

    public enum OrderStatus
    {
        Processing = 1,
        Delivered = 2,
        Returned = 3,
        Cancelled = 4
    }
}
