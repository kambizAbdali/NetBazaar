using Microsoft.AspNetCore.Mvc.Rendering;
using NetBazaar.Domain.Entities.Catalog;

namespace NetBazaar.Admin.EndPoint.ViewModels.Catalog
{
    public class CatalogViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CatalogTypeId { get; set; }
        public int CatalogBrandId { get; set; }

        public IEnumerable<SelectListItem>? Types { get; set; }
        public IEnumerable<SelectListItem>? Brands { get; set; }
    }

    public class CatalogDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public string CatalogType { get; set; }
        public string CatalogBrand { get; set; }
    }
}
