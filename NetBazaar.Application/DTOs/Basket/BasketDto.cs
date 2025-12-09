using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Basket
{
    public class BasketDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public List<BasketItemDto> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(item => item.TotalPrice);
        public int TotalItems => Items.Sum(item => item.Quantity);
        public bool IsRemoved { get; private set; }=false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
