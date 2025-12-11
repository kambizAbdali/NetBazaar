using NetBazaar.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string ToPersianString(this PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.Online => "پرداخت آنلاین",
                PaymentMethod.CashOnDelivery => "پرداخت در محل",
                _ => method.ToString()
            };
        }

        public static string ToPersianString(this PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "در انتظار پرداخت",
                PaymentStatus.Paid => "پرداخت شده",
                _ => status.ToString()
            };
        }

        public static string ToPersianString(this OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Processing => "در حال پردازش",
                OrderStatus.Delivered => "تحویل شده",
                OrderStatus.Returned => "مرجوع شده",
                OrderStatus.Cancelled => "لغو شده",
                _ => status.ToString()
            };
        }
    }
}
