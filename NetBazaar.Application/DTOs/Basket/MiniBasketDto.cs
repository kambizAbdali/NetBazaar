using System;
using System.Collections.Generic;
using System.Text;
using static NetBazaar.Application.Interfaces.Basket.IBasketService;

namespace NetBazaar.Application.DTOs.Basket
{
    public class MiniBasketDto
    {
        public int ItemsCount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<MiniBasketItemDto> Items { get; set; } = new();
    }
}
