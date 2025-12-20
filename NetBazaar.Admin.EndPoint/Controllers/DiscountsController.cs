using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBazaar.Admin.EndPoint.ViewModels.Discounts;
using NetBazaar.Application.DTOs.Common;
using NetBazaar.Application.DTOs.Discounts;
using NetBazaar.Application.Interfaces;
using NetBazaar.Application.Interfaces.Catalog;
using NetBazaar.Domain.Discounts;
using System.Linq;
using System.Threading.Tasks;

namespace NetBazaar.Admin.EndPoint.Controllers
{
    [Route("discounts")]
    public class DiscountsController : BaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ICatalogItemService _catalogItemService;

        public DiscountsController(
            IDiscountService discountService,
            ICatalogItemService catalogItemService)
        {
            _discountService = discountService;
            _catalogItemService = catalogItemService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = DefaultPageSize)
        {
            var list = await _discountService.GetListAsync(page, pageSize);
            return View(list);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var vm = new CreateDiscountViewModel();
            await PopulateCatalogItems(vm);
            return View(vm);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest("شناسه معتبر نمی‌باشد");

            var discount = await _discountService.GetByIdAsync(id);
            if (discount == null) return NotFound();
            return View(discount);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDiscountViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCatalogItems(vm);
                return View(vm);
            }

            var catalogItemIds = vm.SelectedCatalogItemIds?.Any() == true
                ? vm.SelectedCatalogItemIds
                : null;

            var dto = new CreateDiscountDto
            {
                Name = vm.Name,
                UsePercentage = vm.UsePercentage,
                DiscountPercentage = vm.DiscountPercentage,
                DiscountAmount = vm.DiscountAmount,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                RequiresCouponCode = vm.RequiresCouponCode,
                CouponCode = vm.CouponCode,
                DiscountType = vm.DiscountType,
                LimitationType = vm.LimitationType,
                LimitationTimes = vm.LimitationTimes,
                CatalogItemIds = catalogItemIds
            };

            var id = await _discountService.CreateAsync(dto);
            TempData[OperationSuccessKey] = "تخفیف با موفقیت ایجاد شد";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest("شناسه معتبر نمی‌باشد");

            var discount = await _discountService.GetByIdAsync(id);
            if (discount == null) return NotFound();

            var vm = new CreateDiscountViewModel
            {
                Name = discount.Name,
                UsePercentage = discount.UsePercentage,
                DiscountPercentage = discount.DiscountPercentage,
                DiscountAmount = discount.DiscountAmount,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                RequiresCouponCode = discount.RequiresCouponCode,
                CouponCode = discount.CouponCode,
                DiscountType = discount.DiscountType,
                LimitationType = discount.LimitationType,
                LimitationTimes = discount.LimitationTimes,
                SelectedCatalogItemIds = discount.CatalogItems?.Select(ci => ci.Id).ToList(),
                SelectedCatalogItemNames = discount.CatalogItems?.Select(ci => ci.Name).ToList()
            };

            await PopulateCatalogItems(vm);
            return View(vm);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateDiscountViewModel vm)
        {
            if (id <= 0) return BadRequest("شناسه معتبر نمی‌باشد");

            if (!ModelState.IsValid)
            {
                await PopulateCatalogItems(vm);
                return View(vm);
            }

            var catalogItemIds = vm.SelectedCatalogItemIds?.Any() == true
                ? vm.SelectedCatalogItemIds
                : null;

            var dto = new CreateDiscountDto
            {
                Name = vm.Name,
                UsePercentage = vm.UsePercentage,
                DiscountPercentage = vm.DiscountPercentage,
                DiscountAmount = vm.DiscountAmount,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                RequiresCouponCode = vm.RequiresCouponCode,
                CouponCode = vm.CouponCode,
                DiscountType = vm.DiscountType,
                LimitationType = vm.LimitationType,
                LimitationTimes = vm.LimitationTimes,
                CatalogItemIds = catalogItemIds
            };

            await _discountService.UpdateAsync(id, dto);
            TempData[OperationSuccessKey] = "تخفیف با موفقیت بروزرسانی شد";
            return RedirectToAction(nameof(Details), new { id });
        }

        // متد کمکی برای بارگذاری محصولات
        private async Task PopulateCatalogItems(CreateDiscountViewModel vm)
        {
            var products = await _catalogItemService.GetItemsForDropdownAsync();

            vm.CatalogItems = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} - {p.Price:N0} ریال",
                Selected = vm.SelectedCatalogItemIds?.Contains(p.Id) ?? false
            }).ToList();
        }
    }
}
