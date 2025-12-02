namespace NetBazaar.Web.EndPoint.ViewModels
{
    public class PaginationViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }

    public class CatalogFilterViewModel
    {
        public List<int> CategoryIds { get; set; } = new();
        public List<int> BrandIds { get; set; } = new();
        public long? MinPrice { get; set; }
        public long? MaxPrice { get; set; }
        public string SortBy { get; set; } = "newest";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }

    public class BrandItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CategoryItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public enum StockStatus
    {
        InStock = 1,
        LimitedStock = 2,
        OutOfStock = 3,
        ComingSoon = 4,
        Discontinued = 5
    }

    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;

        public string MainImageUrl { get; set; } = string.Empty;

        public int? DiscountPercentage { get; set; }
        public bool HasDiscount => DiscountPercentage.HasValue && DiscountPercentage.Value > 0;
        public string DisplayPrice { get; set; } = string.Empty;
        public string OriginalPrice { get; set; } = string.Empty;

        public double Rating { get; set; }           // 0..5
        public int ReviewCount { get; set; }
        public bool IsFeatured { get; set; }
        public StockStatus StockStatus { get; set; }
        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
    }

    public class CatalogListViewModel
    {
        public List<ProductViewModel> Products { get; set; } = new();
        public List<BrandItemViewModel> Brands { get; set; } = new();
        public List<CategoryItemViewModel> Categories { get; set; } = new();
        public CatalogFilterViewModel Filter { get; set; } = new();
        public PaginationViewModel Pagination { get; set; } = new();
    }
}
