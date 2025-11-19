using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetBazaar.Web.EndPoint.ViewModels
{
    public class CatalogListViewModel
    {
        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public CatalogFilterViewModel Filter { get; set; } = new CatalogFilterViewModel();
        public PaginationInfoViewModel Pagination { get; set; } = new PaginationInfoViewModel();
    }

    public class ProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "نام محصول")]
        public string Name { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات کامل")]
        public string Description { get; set; }

        [Display(Name = "قیمت")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "قیمت با تخفیف")]
        [DataType(DataType.Currency)]
        public decimal? DiscountedPrice { get; set; }

        [Display(Name = "تصویر اصلی")]
        public string MainImageUrl { get; set; }

        [Display(Name = "تصاویر")]
        public List<string> ImageUrls { get; set; } = new List<string>();

        [Display(Name = "دسته بندی")]
        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        [Display(Name = "تعداد موجودی")]
        public int StockQuantity { get; set; }

        [Display(Name = "وضعیت موجودی")]
        public StockStatus StockStatus { get; set; }

        [Display(Name = "امتیاز")]
        public double Rating { get; set; }

        [Display(Name = "تعداد نظرات")]
        public int ReviewCount { get; set; }

        [Display(Name = "ویژه")]
        public bool IsFeatured { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "برچسب ها")]
        public List<string> Tags { get; set; } = new List<string>();

        // SEO Properties
        [Display(Name = "عنوان متا")]
        public string MetaTitle { get; set; }

        [Display(Name = "توضیحات متا")]
        public string MetaDescription { get; set; }

        [Display(Name = "اسلاگ")]
        public string Slug { get; set; }

        // Computed Properties
        public bool HasDiscount => DiscountedPrice.HasValue && DiscountedPrice > 0;
        public decimal DiscountPercentage => HasDiscount ?
            Math.Round(((Price - DiscountedPrice.Value) / Price) * 100, 0) : 0;

        public string DisplayPrice => HasDiscount ?
            DiscountedPrice.Value.ToString("N0") : Price.ToString("N0");

        public string OriginalPrice => HasDiscount ? Price.ToString("N0") : null;
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "نام دسته بندی")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }

        [Display(Name = "والد")]
        public int? ParentId { get; set; }

        [Display(Name = "والد")]
        public string ParentName { get; set; }

        [Display(Name = "ترتیب نمایش")]
        public int DisplayOrder { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "تعداد محصولات")]
        public int ProductCount { get; set; }

        [Display(Name = "اسلاگ")]
        public string Slug { get; set; }

        // SEO Properties
        [Display(Name = "عنوان متا")]
        public string MetaTitle { get; set; }

        [Display(Name = "توضیحات متا")]
        public string MetaDescription { get; set; }

        // Navigation
        public List<CategoryViewModel> Children { get; set; } = new List<CategoryViewModel>();
    }

    public class CatalogFilterViewModel
    {
        [Display(Name = "جستجو")]
        public string Search { get; set; }

        [Display(Name = "حداقل قیمت")]
        [DataType(DataType.Currency)]
        public decimal? MinPrice { get; set; }

        [Display(Name = "حداکثر قیمت")]
        [DataType(DataType.Currency)]
        public decimal? MaxPrice { get; set; }

        [Display(Name = "دسته بندی ها")]
        public List<int> CategoryIds { get; set; } = new List<int>();

        [Display(Name = "برچسب ها")]
        public List<string> Tags { get; set; } = new List<string>();

        [Display(Name = "مرتب سازی")]
        public string SortBy { get; set; } = "newest";

        [Display(Name = "فقط محصولات ویژه")]
        public bool OnlyFeatured { get; set; }

        [Display(Name = "فقط محصولات موجود")]
        public bool OnlyInStock { get; set; }

        [Display(Name = "فقط محصولات تخفیف دار")]
        public bool OnlyDiscounted { get; set; }

        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;

        // Computed Properties
        public bool HasFilters =>
            !string.IsNullOrEmpty(Search) ||
            MinPrice.HasValue ||
            MaxPrice.HasValue ||
            CategoryIds.Any() ||
            Tags.Any() ||
            OnlyFeatured ||
            OnlyInStock ||
            OnlyDiscounted;
    }

    public class PaginationInfoViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public int StartPage
        {
            get
            {
                var startPage = PageNumber - 2;
                if (startPage <= 0)
                    startPage = 1;
                return startPage;
            }
        }

        public int EndPage
        {
            get
            {
                var endPage = PageNumber + 2;
                if (endPage > TotalPages)
                    endPage = TotalPages;
                return endPage;
            }
        }

        public IEnumerable<int> Pages => Enumerable.Range(StartPage, EndPage - StartPage + 1);
    }

    public enum StockStatus
    {
        [Display(Name = "ناموجود")]
        OutOfStock = 0,

        [Display(Name = "موجود")]
        InStock = 1,

        [Display(Name = "Limited")]
        LimitedStock = 2,

        [Display(Name = "به زودی")]
        ComingSoon = 3,

        [Display(Name = "Discontinued")]
        Discontinued = 4
    }
}