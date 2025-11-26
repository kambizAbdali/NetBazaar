using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Catalog
{
    public class CatalogTypeDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public int? ParentId { get; set; }

        // ✅ اضافه‌شده: لیست فرزندان برای ساختار درختی
        public List<CatalogTypeDto> Children { get; set; } = new();
    }
}