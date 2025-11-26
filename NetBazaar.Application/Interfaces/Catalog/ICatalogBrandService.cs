using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Catalog
{
    public interface ICatalogBrandService
    {
        Task<PaginatedResult<CatalogBrandListDto>> GetListAsync(int page, int pageSize);
        Task<CatalogBrandDto?> GetByIdAsync(int id);
        Task AddAsync(CatalogBrandDto dto);
        Task EditAsync(int id, CatalogBrandDto dto);
        Task RemoveAsync(int id);
    }
}
