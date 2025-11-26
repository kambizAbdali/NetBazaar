using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.DTOs.Common;
using NetBazaar.Application.Interfaces.Catalog;
using NetBazaar.Domain.Entities.Catalog;
using NetBazaar.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NetBazaar.Infrastructure.Services.Catalog
{
    public class CatalogItemService : ICatalogItemService
    {
        private readonly NetBazaarDbContext _context;
        private readonly IMapper _mapper;

        public CatalogItemService(NetBazaarDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> AddAsync(AddCatalogItemDto dto)
        {
            var entity = _mapper.Map<CatalogItem>(dto);

            await _context.CatalogItems.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<PaginatedResult<CatalogListDto>> GetListAsync(int page, int pageSize)
        {
            var totalCount = await _context.CatalogItems.CountAsync();

            var query = _context.CatalogItems
                .Include(c => c.CatalogType)
                .Include(c => c.CatalogBrand)
                .AsNoTracking();

            var data = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ProjectTo<CatalogListDto>(_mapper.ConfigurationProvider)
                                  .ToListAsync();

            return new PaginatedResult<CatalogListDto>
            {
                Data = data,
                PageIndex = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<CatalogDto?> GetByIdAsync(long id)
        {
            var item = await _context.CatalogItems
                .Include(c => c.CatalogType)
                .Include(c => c.CatalogBrand)
                .Include(c => c.Images)
                .Include(c => c.Features)
                .FirstOrDefaultAsync(c => c.Id == id);

            return item == null ? null : _mapper.Map<CatalogDto>(item);
        }

        public async Task EditAsync(long id, CatalogDto dto)
        {
            var entity = await _context.CatalogItems
                .Include(x => x.Images)
                .Include(x => x.Features)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) throw new KeyNotFoundException("CatalogItem not found");

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Price = (long)dto.Price;
            entity.CatalogTypeId = dto.CatalogTypeId;
            entity.CatalogBrandId = dto.CatalogBrandId;

            entity.Features = dto.Features.Select(f => new CatalogItemFeature
            {
                Group = f.Group,
                Key = f.Key,
                Value = f.Value
            }).ToList();
            entity.Images = dto.Images.Select(i => new CatalogItemImage
            {
                Src = i.Src
            }).ToList();
            _context.CatalogItems.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(long id)
        {
            var entity = await _context.CatalogItems.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException("CatalogItem not found");

            _context.CatalogItems.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
