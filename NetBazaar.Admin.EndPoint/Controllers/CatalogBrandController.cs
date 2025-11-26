using Microsoft.AspNetCore.Mvc;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.Interfaces.Catalog;
using Microsoft.Extensions.Logging;
using NetBazaar.Admin.EndPoint.ViewModels.Catalog;

namespace NetBazaar.Admin.EndPoint.Controllers
{
    [Route("[controller]")]
    public class CatalogBrandController : BaseController
    {
        private readonly ICatalogBrandService _catalogBrandService;
        private readonly ILogger<CatalogBrandController> _logger;
        public CatalogBrandController(
            ICatalogBrandService catalogBrandService,
            ILogger<CatalogBrandController> logger)
        {
            _catalogBrandService = catalogBrandService ?? throw new ArgumentNullException(nameof(catalogBrandService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = DefaultPageSize)
        {
            try
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 100);

                var result = await _catalogBrandService.GetListAsync(page, pageSize);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving catalog brands list. Page: {Page}, PageSize: {PageSize}", page, pageSize);
                TempData[OperationErrorKey] = $"خطا در دریافت لیست برندها \n{ex.Message}";
                return View(new List<CatalogBrandDto>());
            }
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new CatalogBrandViewModel());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatalogBrandViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var dto = new CatalogBrandDto { Brand = model.Brand?.Trim() };
                await _catalogBrandService.AddAsync(dto);

                TempData[OperationSuccessKey] = "برند با موفقیت ایجاد شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating catalog brand. Brand: {Brand}", model.Brand);
                TempData[OperationErrorKey] = $"خطا در ایجاد برند \n{ex.Message}";
                return View(model);
            }
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest($"برند با شناسه {id} معتبر نمی باشد");

            try
            {
                var dto = await _catalogBrandService.GetByIdAsync(id);
                if (dto == null) return NotFound();

                var vm = new CatalogBrandViewModel { Id = dto.Id, Brand = dto.Brand };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving brand for edit. ID: {Id}", id);
                TempData[OperationErrorKey] = $"خطا در دریافت اطلاعات برند \n{ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CatalogBrandViewModel model)
        {
            if (id != model.Id) return BadRequest("شناسه های درخواست مطابقت ندارند");
            if (!ModelState.IsValid) return View(model);

            try
            {
                var dto = new CatalogBrandDto { Id = model.Id, Brand = model.Brand?.Trim() };
                await _catalogBrandService.EditAsync(id, dto);

                TempData[OperationSuccessKey] = "برند با موفقیت ویرایش شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating brand. ID: {Id}", id);
                TempData[OperationErrorKey] = $"خطا در ویرایش برند \n{ex.Message}";
                return View(model);
            }
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("شناسه معتبر نمی باشد");

            try
            {
                var dto = await _catalogBrandService.GetByIdAsync(id);
                if (dto == null) return NotFound();

                var vm = new CatalogBrandViewModel { Id = dto.Id, Brand = dto.Brand };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving brand for deletion. ID: {Id}", id);
                TempData[OperationErrorKey] = $"خطا در حذف برند با شناسه {id}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0) return BadRequest("شناسه معتبر نمی باشد");

            try
            {
                await _catalogBrandService.RemoveAsync(id);
                TempData[OperationSuccessKey] = $"برند با شناسه {id} با موفقیت حذف شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting brand. ID: {Id}", id);
                TempData[OperationErrorKey] = $"خطا در حذف برند با شناسه {id}. لطفاً دوباره تلاش کنید";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
