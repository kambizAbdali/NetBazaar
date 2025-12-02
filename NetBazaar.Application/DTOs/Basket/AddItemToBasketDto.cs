using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Basket
{
    public class AddItemToBasketDto
    {
        public int CatalogItemId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
