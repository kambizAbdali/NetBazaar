using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Basket
{
    public class UpdateBasketItemQuantityDto
    {
        public int BasketItemId { get; set; }
        public int Quantity { get; set; }
    }
}
