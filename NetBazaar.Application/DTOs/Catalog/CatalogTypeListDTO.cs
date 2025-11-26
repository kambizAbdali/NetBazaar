using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Catalog
{
    public class CatalogTypeListDTO
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public int ChildrenCount { get; set; }
    }
}