using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Catalog
{
    public interface ICatalogItemService
    {
        Task<long> AddAsync(AddCatalogItemDto dto);
        Task<PaginatedResult<CatalogListDto>> GetListAsync(int page, int pageSize);
        Task<CatalogDto?> GetByIdAsync(long id);
        Task EditAsync(long id, CatalogDto dto);
        Task RemoveAsync(long id);
    }
}
