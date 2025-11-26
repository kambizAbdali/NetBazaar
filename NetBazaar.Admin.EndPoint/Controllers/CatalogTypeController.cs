using Microsoft.AspNetCore.Mvc;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.Interfaces.Catalog;
using Microsoft.Extensions.Logging;
using NetBazaar.Admin.EndPoint.ViewModels.Catalog;

namespace NetBazaar.Admin.EndPoint.Controllers
{
    [Route("[controller]")]
    public class CatalogTypeController : BaseController
    {
        private readonly ICatalogTypeService _catalogTypeService;
        private readonly ILogger<CatalogTypeController> _logger;
        public CatalogTypeController(
            ICatalogTypeService catalogTypeService,
            ILogger<CatalogTypeController> logger)
        {
            _catalogTypeService = catalogTypeService ?? throw new ArgumentNullException(nameof(catalogTypeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = DefaultPageSize)
        {
            try
            {
                // Validate pagination parameters
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 100); // Limit page size to prevent abuse

                var result = await _catalogTypeService.GetListAsync(page, pageSize);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving catalog types list. Page: {Page}, PageSize: {PageSize}", page, pageSize);
                TempData[OperationErrorKey] = $"خطا در دریافت لیست انواع کاتالوگ /n{ex.Message}";

                return View(new List<CatalogTypeDto>()); // Return empty list instead of crashing
            }
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new CatalogTypeViewModel());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatalogTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var dto = new CatalogTypeDto
                {
                    Type = model.Type?.Trim(),
                    ParentId = model.ParentId
                };

                await _catalogTypeService.AddAsync(dto);

                TempData[OperationSuccessKey] = "نوع کاتالوگ با موفقیت ایجاد شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating catalog type. Type: {Type}, ParentId: {ParentId}",
                    model.Type, model.ParentId);
                TempData[OperationErrorKey] = $"خطا در ایجاد نوع کاتالوگ. لطفاً دوباره تلاش کنید /n{ex.Message}";
                return View(model);
            }
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest("شناسه معتبر نمی باشد");
            }

            try
            {
                var dto = await _catalogTypeService.GetByIdAsync(id);
                if (dto == null)
                {
                    return NotFound();
                }

                var vm = new CatalogTypeViewModel
                {
                    Id = dto.Id,
                    Type = dto.Type,
                    ParentId = dto.ParentId
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving catalog type for edit. ID: {Id}", id);
                TempData[OperationErrorKey] = $"خطا در دریافت اطلاعات نوع کاتالوگ /n{ex.Message}" ;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CatalogTypeViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("شناسه های درخواست مطابقت ندارند");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var dto = new CatalogTypeDto
                {
                    Id = model.Id,
                    Type = model.Type?.Trim(),
                    ParentId = model.ParentId
                };

                await _catalogTypeService.EditAsync(id, dto);

                TempData[OperationSuccessKey] = "نوع کاتالوگ با موفقیت ویرایش شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating catalog type. ID: {Id}", id);
                ModelState.AddModelError("", "خطا در ویرایش نوع کاتالوگ. لطفاً دوباره تلاش کنید");

                TempData[OperationErrorKey] = $"خطا در ویرایش اطلاعات /n{ex.Message}";
                return View(model);
            }
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("شناسه معتبر نمی باشد");
            }

            try
            {
                var dto = await _catalogTypeService.GetByIdAsync(id);
                if (dto == null)
                {
                    return NotFound();
                }

                var vm = new CatalogTypeViewModel
                {
                    Id = dto.Id,
                    Type = dto.Type,
                    ParentId = dto.ParentId
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving catalog type for deletion confirmation. ID: {Id}", id);
                TempData[OperationErrorKey] = "خطا در دریافت اطلاعات نوع کاتالوگ";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                return BadRequest("شناسه معتبر نمی باشد");
            }

            try
            {
                await _catalogTypeService.RemoveAsync(id);

                TempData[OperationSuccessKey] = $" دسته بندی با شناسه {id} با موفقیت حذف شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting catalog type. ID: {Id}", id);
                TempData[OperationErrorKey] = "خطا در حذف نوع کاتالوگ. لطفاً دوباره تلاش کنید";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}