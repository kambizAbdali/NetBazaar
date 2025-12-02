using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.DTOs.Common;
using NetBazaar.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Catalog
{
    public interface ICatalogTypeService
    {
        Task<PaginatedResult<CatalogTypeListDTO>> GetListAsync(int pageIndex, int pageSize);
        Task<CatalogTypeDto?> GetByIdAsync(int id);
        Task<bool> AddAsync(CatalogTypeDto dto);
        Task<bool> EditAsync(int id, CatalogTypeDto dto);
        Task<bool> RemoveAsync(int id);


        // ✅ متد جدید برای گرفتن همه دسته‌بندی‌ها
        Task<List<CatalogTypeDto>> GetAllAsync();
        Task<List<CatalogTypeDto>> GetAllDescendantsOptimizedAsync(int categoryId);
    }
}
