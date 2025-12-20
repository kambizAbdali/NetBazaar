using NetBazaar.Domain.Discounts;
using NetBazaar.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
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

        public static string? GetDisplayName(this Enum value)
        {
            if (value == null) return null;

            // Find the member on the enum that matches the value
            var memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length == 0) return value.ToString();

            // Look for DisplayAttribute on the enum field
            var displayAttr = memberInfo[0]
                .GetCustomAttribute<DisplayAttribute>();

            // Return the attribute's Name if present, otherwise fallback to the enum name
            return displayAttr?.Name ?? value.ToString();
        }

        public static IEnumerable<(int Value, string DisplayName)> GetItemsWithDisplayName<TEnum>() where TEnum : struct, Enum
        {
            foreach (var v in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
            {
                yield return (Convert.ToInt32(v), v.GetDisplayName());
            }
        }
    }
}
