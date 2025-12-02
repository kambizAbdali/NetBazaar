using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.Catalog
{
    public class CatalogItemImage
    {
        public int Id { get; set; }
        public long CatalogItemId { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public string Src { get; set; } = string.Empty; // مسیر یا نام فایل
        public int? SortOrder { get; set; }              // ترتیب برای گالری
    }
}
