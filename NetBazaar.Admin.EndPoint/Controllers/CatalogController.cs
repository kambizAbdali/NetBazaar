using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBazaar.Admin.EndPoint.ViewModels.Catalog;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.Interfaces.Catalog;
using System.ComponentModel.DataAnnotations;

namespace NetBazaar.Admin.EndPoint.Controllers
{
    public class CatalogController : BaseController
    {
        private readonly ICatalogItemService _catalogItemService;
        private readonly ICatalogTypeService _catalogTypeService;
        private readonly ICatalogBrandService _catalogBrandService;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(
            ICatalogItemService catalogItemService,
            ICatalogTypeService catalogTypeService,
            ICatalogBrandService catalogBrandService,
            ILogger<CatalogController> logger)
        {
            _catalogItemService = catalogItemService;
            _catalogTypeService = catalogTypeService;
            _catalogBrandService = catalogBrandService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = DefaultPageSize)
        {
            try
            {
                var result = await _catalogItemService.GetListAsync(page, pageSize);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving catalog list for page {Page} with page size {PageSize}", page, pageSize);
                TempData[OperationErrorKey] = $"خطا در دریافت لیست کاتالوگ\n{ex.Message}";
                return View(new List<CatalogListDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            if (id <= 0)
            {
                return BadRequest("شناسه معتبر نمی باشد");
            }

            try
            {
                var item = await _catalogItemService.GetByIdAsync(id);
                if (item == null)
                {
                    TempData[OperationErrorKey] = $"محصول با شناسه {id} یافت نشد";
                    return RedirectToAction(nameof(Index));
                }
                return View(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving catalog item details for ID {CatalogItemId}", id);
                TempData[OperationErrorKey] = $"خطا در دریافت اطلاعات جزئیات محصول با شناسه {id}\n{ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var viewModel = await InitializeCreateViewModel();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing create catalog item page");
                TempData[OperationErrorKey] = $"خطا در حین ورود به صفحه ایجاد محصول جدید\n{ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCatalogItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }

            try
            {
                var dto = MapToAddCatalogItemDto(viewModel);
                await _catalogItemService.AddAsync(dto);

                TempData[OperationSuccessKey] = "محصول با موفقیت اضافه شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating catalog item");
                TempData[OperationErrorKey] = $"خطا در ایجاد محصول. لطفاً دوباره تلاش کنید\n{ex.Message}";
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            if (id <= 0)
            {
                return BadRequest("شناسه معتبر نمی باشد");
            }

            try
            {
                var item = await _catalogItemService.GetByIdAsync(id);
                if (item == null)
                {
                    TempData[OperationErrorKey] = $"محصول با شناسه {id} یافت نشد";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = await MapToEditViewModel(item);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing edit page for catalog item ID {CatalogItemId}", id);
                TempData[OperationErrorKey] = $"خطا در حین ورود به صفحه ویرایش محصول با کد {id}. لطفاً دوباره تلاش کنید\n{ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, AddCatalogItemViewModel viewModel)
        {
            if (id <= 0)
            {
                return BadRequest("شناسه معتبر نمی باشد");
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }

            try
            {
                var dto = MapToCatalogDto(viewModel, id);
                await _catalogItemService.EditAsync(id, dto);

                TempData[OperationSuccessKey] = "محصول با موفقیت ویرایش شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating catalog item ID {CatalogItemId}", id);
                TempData[OperationErrorKey] = $"خطا در حین ویرایش محصول با کد {id}. لطفاً دوباره تلاش کنید\n{ex.Message}";
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            if (id <= 0)
            {
                return BadRequest("شناسه معتبر نمی باشد");
            }

            try
            {
                var item = await _catalogItemService.GetByIdAsync(id);
                if (item == null)
                {
                    TempData[OperationErrorKey] = $"محصول با شناسه {id} یافت نشد";
                    return RedirectToAction(nameof(Index));
                }
                return View(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing delete page for catalog item ID {CatalogItemId}", id);
                TempData[OperationErrorKey] = $"خطا در حین ورود به صفحه حذف محصول با کد {id}. لطفاً دوباره تلاش کنید\n{ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("delete/{id:long}")]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (id <= 0)
            {
                return BadRequest("شناسه معتبر نمی باشد");
            }

            try
            {
                await _catalogItemService.RemoveAsync(id);
                TempData[OperationSuccessKey] = $"محصول با شناسه {id} با موفقیت حذف شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting catalog item ID {CatalogItemId}", id);
                TempData[OperationErrorKey] = "خطا در حذف محصول. لطفاً دوباره تلاش کنید";
                return RedirectToAction(nameof(Index));
            }
        }

        #region Private Methods

        private async Task<AddCatalogItemViewModel> InitializeCreateViewModel()
        {
            var types = await _catalogTypeService.GetAllAsync();
            var selectItems = new List<SelectListItem>();
            FlattenCatalogTypes(types, selectItems);

            return new AddCatalogItemViewModel
            {
                Types = selectItems,
                Brands = (await _catalogBrandService.GetListAsync(1, 100)).Data
                    .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Brand })
            };
        }

        private void FlattenCatalogTypes(List<CatalogTypeDto> types, List<SelectListItem> items, string prefix = "")
        {
            foreach (var type in types)
            {
                items.Add(new SelectListItem
                {
                    Value = type.Id.ToString(),
                    Text = prefix + type.Type
                });

                if (type.Children?.Any() == true)
                {
                    FlattenCatalogTypes(type.Children, items, prefix + "__ ");
                }
            }
        }

        private async Task PopulateDropdowns(AddCatalogItemViewModel viewModel)
        {
            var types = await _catalogTypeService.GetAllAsync();
            var selectItems = new List<SelectListItem>();
            FlattenCatalogTypes(types, selectItems);

            viewModel.Types = selectItems;
            viewModel.Brands = (await _catalogBrandService.GetListAsync(1, 100)).Data
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Brand });
        }

        private async Task<AddCatalogItemViewModel> MapToEditViewModel(CatalogDto item)
        {
            var types = await _catalogTypeService.GetAllAsync();
            var selectItems = new List<SelectListItem>();
            FlattenCatalogTypes(types, selectItems);

            return new AddCatalogItemViewModel
            {
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                CatalogTypeId = item.CatalogTypeId,
                CatalogBrandId = item.CatalogBrandId,
                StockQuantity = item.StockQuantity,
                ReorderThreshold = item.ReorderThreshold,
                MaxStockThreshold = item.MaxStockThreshold,
                Features = item.Features.Select(f => new CatalogItemFeatureViewModel
                {
                    Group = f.Group,
                    Key = f.Key,
                    Value = f.Value
                }).ToList(),
                Images = item.Images.Select(i => new CatalogItemImageViewModel
                {
                    Src = i.Src
                }).ToList(),
                Types = selectItems,
                Brands = (await _catalogBrandService.GetListAsync(1, 100)).Data
                    .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Brand })
            };
        }

        private static AddCatalogItemDto MapToAddCatalogItemDto(AddCatalogItemViewModel viewModel)
        {
            return new AddCatalogItemDto
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                CatalogTypeId = viewModel.CatalogTypeId,
                BrandId = viewModel.CatalogBrandId,
                StockQuantity = viewModel.StockQuantity,
                ReorderThreshold = viewModel.ReorderThreshold,
                MaxStockThreshold = viewModel.MaxStockThreshold,
                Features = viewModel.Features?.Select(f => new CatalogItemFeatureDto
                {
                    Group = f.Group,
                    Key = f.Key,
                    Value = f.Value
                }).ToList() ?? new List<CatalogItemFeatureDto>(),
                Images = viewModel.Images?.Select(i => new CatalogItemImageDto
                {
                    Src = i.Src
                }).ToList() ?? new List<CatalogItemImageDto>()
            };
        }

        private static CatalogDto MapToCatalogDto(AddCatalogItemViewModel viewModel, long id)
        {
            return new CatalogDto
            {
                Id = (int)id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                CatalogTypeId = viewModel.CatalogTypeId,
                CatalogBrandId = viewModel.CatalogBrandId,
                StockQuantity = viewModel.StockQuantity,
                ReorderThreshold = viewModel.ReorderThreshold,
                MaxStockThreshold = viewModel.MaxStockThreshold,
                Features = viewModel.Features?.Select(f => new CatalogItemFeatureDto
                {
                    Group = f.Group,
                    Key = f.Key,
                    Value = f.Value
                }).ToList() ?? new List<CatalogItemFeatureDto>(),
                Images = viewModel.Images?.Select(i => new CatalogItemImageDto
                {
                    Src = i.Src
                }).ToList() ?? new List<CatalogItemImageDto>()
            };
        }

        #endregion
    }
}