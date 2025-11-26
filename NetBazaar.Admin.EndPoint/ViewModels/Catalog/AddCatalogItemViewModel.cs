using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetBazaar.Admin.EndPoint.ViewModels.Catalog
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using System.ComponentModel.DataAnnotations;

    public class AddCatalogItemViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "نام محصول اجباری است")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "نام محصول باید بین ۲ تا ۱۰۰ کاراکتر باشد")]
        public string Name { get; set; }

        [Required(ErrorMessage = "توضیحات محصول اجباری است")]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "قیمت باید بزرگتر از صفر باشد")]
        public long Price { get; set; }

        [Range(0, 100000, ErrorMessage = "موجودی باید بین ۰ تا ۱۰۰,۰۰۰ باشد")]
        public int StockQuantity { get; set; }
        [Required(ErrorMessage = "نوع محصول اجباری است")]

        public int CatalogTypeId { get; set; }
        [Required(ErrorMessage = "برند محصول اجباری است")]

        public int CatalogBrandId { get; set; }

        [Range(1, 1000, ErrorMessage = "حداقل موجودی برای هشدار باید بین ۱ تا ۱۰۰,۰۰۰ باشد")]
        public int ReorderThreshold { get; set; }

        [Range(0, 100000, ErrorMessage = "حداکثر ظرفیت انبار باید بین ۰ تا ۱۰۰,۰۰۰ باشد")]
        public int MaxStockThreshold { get; set; }
        public List<CatalogItemFeatureViewModel> Features { get; set; } = new();
        public List<CatalogItemImageViewModel> Images { get; set; } = new();

        [ValidateNever]
        public IEnumerable<SelectListItem> Types { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Brands { get; set; }
    }

    public class CatalogItemFeatureViewModel
    {
        public string Group { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class CatalogItemImageViewModel
    {
        public string Src { get; set; } = string.Empty;
    }
}
