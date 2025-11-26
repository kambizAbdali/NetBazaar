using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.Catalog
{
    public class CatalogType
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public CatalogType? Parent { get; set; }
        public ICollection<CatalogType> Children { get; set; } = new List<CatalogType>();

        // ارتباط با محصولات
        public ICollection<CatalogItem> CatalogItems { get; set; } = new List<CatalogItem>();
    }
}
