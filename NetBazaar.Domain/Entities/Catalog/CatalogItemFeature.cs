using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.Catalog
{
    public class CatalogItemFeature
    {
        public int Id { get; set; }
        public string Group { get; set; } = string.Empty; // گروه ویژگی (مثلاً "پردازنده")
        public string Key { get; set; } = string.Empty;   // نام ویژگی (مثلاً "مدل پردازنده")
        public string Value { get; set; } = string.Empty; // مقدار ویژگی (مثلاً "Intel Core i7")

        public long CatalogItemId { get; set; }
        public CatalogItem CatalogItem { get; set; }
    }
}