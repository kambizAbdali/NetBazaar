using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.DTOs.Common;
using NetBazaar.Application.Interfaces.Catalog;
using NetBazaar.Domain.Entities.Catalog;
using NetBazaar.Infrastructure.Data;
using NetBazaar.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetBazaar.Infrastructure.Services.Catalog
{
    public class CatalogBrandService : ICatalogBrandService
    {
        private readonly NetBazaarDbContext _context;

        public CatalogBrandService(NetBazaarDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<CatalogBrandListDto>> GetListAsync(int page, int pageSize)
        {
            var query = _context.CatalogBrands.AsNoTracking();

            var totalCount = await query.CountAsync();
            var data = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .Select(b => new CatalogBrandListDto
                                  {
                                      Id = b.Id,
                                      Brand = b.Brand
                                  })
                                  .ToListAsync();
            return new PaginatedResult<CatalogBrandListDto>
            {
                Data = data,
                PageIndex = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<CatalogBrandDto?> GetByIdAsync(int id)
        {
            var brand = await _context.CatalogBrands.FindAsync(id);
            if (brand == null) return null;

            return new CatalogBrandDto { Id = brand.Id, Brand = brand.Brand };
        }

        public async Task AddAsync(CatalogBrandDto dto)
        {
            var brand = new CatalogBrand { Brand = dto.Brand };
            _context.CatalogBrands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, CatalogBrandDto dto)
        {
            var brand = await _context.CatalogBrands.FindAsync(id);
            if (brand == null) throw new KeyNotFoundException("Brand not found");

            brand.Brand = dto.Brand;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var brand = await _context.CatalogBrands.FindAsync(id);
            if (brand == null) throw new KeyNotFoundException("Brand not found");

            _context.CatalogBrands.Remove(brand);
            await _context.SaveChangesAsync();
        }
    }
}
