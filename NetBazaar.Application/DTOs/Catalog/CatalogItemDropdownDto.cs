using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Catalog
{
    // در پروژه Infrastructure یا Application
    public class CatalogItemDropdownDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
    }
}
