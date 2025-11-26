using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.Catalog
{
    public class CatalogBrand
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;

        // ارتباط با محصولات
        public ICollection<CatalogItem> Catalogs { get; set; } = new List<CatalogItem>();
    }
}
