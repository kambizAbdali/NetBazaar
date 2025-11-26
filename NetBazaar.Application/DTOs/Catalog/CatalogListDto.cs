using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Catalog
{
    public class CatalogListDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string BrandName{ get; set; } = string.Empty;
    }
}
