using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.Catalog
{
    public class CatalogItemImage
    {
        public int Id { get; set; }
        public string Src { get; set; } = string.Empty; // آدرس تصویر

        public long CatalogItemId { get; set; }
        public CatalogItem CatalogItem { get; set; }
    }
}
