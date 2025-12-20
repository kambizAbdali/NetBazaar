using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.DTOs.Common;
using NetBazaar.Application.DTOs.Discounts;
using NetBazaar.Domain.Discounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetBazaar.Application.Interfaces
{
    public interface IDiscountService
    {
        Task<int> CreateAsync(CreateDiscountDto dto);
        Task UpdateAsync(int id, CreateDiscountDto dto);
        Task<Discount?> GetByIdAsync(int id);
        Task<List<DiscountListItemDto>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<PaginatedResult<DiscountListItemDto>> GetListAsync(int page, int pageSize);
    }
}
