using Microsoft.AspNetCore.Mvc;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.Interfaces.Basket;
using NetBazaar.Application.Interfaces.Catalog;
using NetBazaar.EndPoint.Controllers;
using NetBazaar.Infrastructure.Services.Catalog;
using NetBazaar.Web.EndPoint.Utilities;
using NetBazaar.Web.EndPoint.ViewModels;

public class ProductController : BaseController
{
    private readonly IGetCatalogItemPLPService _plpService;
    private readonly ICatalogBrandService _brandService;
    private readonly IGetCatalogItemDetailService _CatalogDetailService;
    private readonly ICatalogTypeService _catalogTypeService;
    private readonly IBasketService _basketService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductController(
        IGetCatalogItemPLPService plpService,
        ICatalogBrandService brandService,
        IGetCatalogItemDetailService catalogDetailService,
        ICatalogTypeService catalogTypeService,
        IBasketService basketService,
        IHttpContextAccessor httpContextAccessor)
    {
        _plpService = plpService;
        _brandService = brandService;
        _CatalogDetailService = catalogDetailService;
        _catalogTypeService = catalogTypeService;
        _basketService = basketService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        int categoryId,
        List<int>? brandIds,
        long? minPrice,
        long? maxPrice,
        string sortBy = "newest",
        int page = 1,
        int pageSize = 12)
    {
        try
        {

            var allDescendantsCategoryIds = (await _catalogTypeService.GetAllDescendantsOptimizedAsync(categoryId)).Select(o => o.Id);

            ShowMenuCategories();
            var pageResult = _plpService.GetCatalogItems(allDescendantsCategoryIds, brandIds, minPrice, maxPrice, sortBy, page, pageSize);

            var brandsVm = (await _brandService.GetListAsync(1, 200)).Data
                .Select(b => new BrandItemViewModel { Id = b.Id, Name = b.Brand }).ToList();

            var productsVm = pageResult.Data.Select(d =>
            {
                var stockStatus = d.StockQuantity > d.ReorderThreshold
                    ? StockStatus.InStock
                    : d.StockQuantity > 0 ? StockStatus.LimitedStock : StockStatus.OutOfStock;

                var originalPrice = d.DiscountPercent.HasValue && d.DiscountPercent.Value > 0
                    ? (Math.Round(d.Price / (1 - (d.DiscountPercent.Value / 100.0))).ToString("N0"))
                    : string.Empty;

                return new ProductViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortDescription = d.Name, // می‌توانی خلاصه از Description بسازی
                    MainImageUrl = d.FirstImageSrc ?? string.Empty,
                    DiscountPercentage = d.DiscountPercent,
                    DisplayPrice = d.Price.ToString("N0"),
                    OriginalPrice = originalPrice,
                    Rating = d.Rating,
                    ReviewCount = d.ReviewCount,
                    
                    IsFeatured = d.IsFeatured,
                    StockStatus = stockStatus,
                    Tags = Enumerable.Empty<string>()
                };
            }).ToList();

            var vm = new CatalogListViewModel
            {
                Products = productsVm,
                Brands = brandsVm,
                Categories = new List<CategoryItemViewModel>(), // در صورت نیاز پر کن
                Filter = new CatalogFilterViewModel
                {
                    CategoryIds = new List<int> { categoryId },
                    BrandIds = brandIds ?? new List<int>(),
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    SortBy = sortBy,
                    Page = page,
                    PageSize = pageSize
                },
                Pagination = new PaginationViewModel
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = pageResult.TotalCount
                }
            };

            return View("Index", vm);
        }
        catch (Exception ex)
        {
            TempData[OperationErrorKey] = $"خطا در بارگذاری لیست محصولات \n{ex.Message}";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]
    public IActionResult ProductDetail(long id)
    {
        var dto = _CatalogDetailService.GetCatalogItemDetail(id);
        if (dto == null)
        {
            return NotFound();
        }
        return View("ProductDetail", dto);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(long productId, int quantity)
    {
        try
        {
            if (quantity <= 0)
            {
                return Json(new { success = false, message = "تعداد باید بیشتر از صفر باشد" });
            }

            var buyerId = BasketHelper.GetOrCreateBuyerId(_httpContextAccessor.HttpContext, User);
            var basket = await _basketService.GetOrCreateBasketForUserAsync(buyerId);

            // بررسی تعداد موجودی محصول در سبد خرید فعلی
            var existingItem = basket.Items.FirstOrDefault(item => item.CatalogItemId == (int)productId);
            var currentQuantityInCart = existingItem?.Quantity ?? 0;
            var requestedTotalQuantity = currentQuantityInCart + quantity;
            int availableQuantity = default;

            // بررسی اینکه مجموع تعداد درخواستی و موجود در سبد از موجودی انبار بیشتر نباشد
            if (requestedTotalQuantity > existingItem.AvailableStock)
            {
                availableQuantity = existingItem.AvailableStock - currentQuantityInCart;

                if (availableQuantity <= 0)
                {
                    TempData["Error"] = "شما حداکثر تعداد قابل خرید این محصول را درحال حاضر در سبد خرید دارید";

                    return Json(new
                    {
                        success = false,
                        message = TempData["Error"]
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = $"تعداد درخواستی بیشتر از موجودی است. تنها {availableQuantity} عدد از این محصول موجود می‌باشد"
                    });
                }
            }

            // افزودن محصول به سبد خرید
            await _basketService.AddItemToBasketAsync(basket.Id, (int)productId, quantity);

            TempData["Success"] = $"محصول با موفقیت به سبد خرید افزوده شد";
            return Json(new
            {
                success = true,
                message = TempData["Success"],
                basketItemsCount = basket.TotalItems + quantity,
                availableStock = availableQuantity - requestedTotalQuantity
            });
        }
        catch (Exception ex)
        {
            TempData["Error"] = "خطا در افزودن محصول به سبد خرید";
            return Json(new { success = false, message = TempData["Error"] });
        }
    }



}
