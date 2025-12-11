using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Basket
{
    using System.ComponentModel.DataAnnotations;

    public class BasketItemDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "شناسه محصول الزامی است.")]
        public long CatalogItemId { get; set; }

        [Required(ErrorMessage = "نام محصول الزامی است.")]
        [MaxLength(200, ErrorMessage = "نام محصول نباید بیشتر از 200 کاراکتر باشد.")]
        public string CatalogItemName { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "آدرس تصویر نباید بیشتر از 500 کاراکتر باشد.")]
        public string CatalogItemImageUrl { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "تعداد باید بیشتر از صفر باشد.")]
        public int Quantity { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "قیمت واحد باید بیشتر از صفر باشد.")]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;

        [MaxLength(100, ErrorMessage = "نام برند نباید بیشتر از 100 کاراکتر باشد.")]
        public string BrandName { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "نام دسته‌بندی نباید بیشتر از 100 کاراکتر باشد.")]
        public string CategoryName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "موجودی باید صفر یا بیشتر باشد.")]
        public int AvailableStock { get; set; }
    }

}
