using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.DTOs.Common;
using NetBazaar.Application.Interfaces.Catalog;
using NetBazaar.Domain.Entities.Catalog;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBazaar.Infrastructure.Services.Catalog
{
    public class CatalogTypeService : ICatalogTypeService
    {
        private readonly INetBazaarDbContext _context;
        private readonly IMapper _mapper;

        public CatalogTypeService(INetBazaarDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<CatalogTypeListDTO>> GetListAsync(int pageIndex, int pageSize)
        {
            var totalCount = await _context.CatalogTypes.CountAsync();

            var data = await _context.CatalogTypes
                .OrderBy(ct => ct.Type)  // System.Linq
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<CatalogTypeListDTO>(_mapper.ConfigurationProvider)// AutoMapper
                .ToListAsync();

            return new PaginatedResult<CatalogTypeListDTO>
            {
                Data = data,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<CatalogTypeDto?> GetByIdAsync(int id)
        {
            var entity = await _context.CatalogTypes.FindAsync(id);
            return entity == null ? null : _mapper.Map<CatalogTypeDto>(entity);
        }

        public async Task<bool> AddAsync(CatalogTypeDto dto)
        {
            var entity = _mapper.Map<CatalogType>(dto);
            _context.CatalogTypes.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditAsync(int id, CatalogTypeDto dto)
        {
            var entity = await _context.CatalogTypes.FindAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await _context.CatalogTypes.FindAsync(id);
            if (entity == null) return false;

            _context.CatalogTypes.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<CatalogType>> GetAllWithHierarchyAsync()
        {
            return await _context.CatalogTypes
                .Include(t => t.Children)
                    .ThenInclude(c1 => c1.Children)
                        .ThenInclude(c2 => c2.Children)
                            .ThenInclude(c3 => c3.Children)
                .Where(t => t.ParentId == null)
                .ToListAsync();
        }

        public async Task<List<CatalogTypeDto>> GetAllAsync()
        {
            var entities = await GetAllWithHierarchyAsync();
            return _mapper.Map<List<CatalogTypeDto>>(entities);
        }

        /// <summary>
        // Retrieves all descendant categories of a given category (BFS algorithm).
        // Returns mapped DTOs; handles missing targets by returning an empty list.
        // Uses eager loading for children and avoids duplicates during traversal.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<List<CatalogTypeDto>> GetAllDescendantsOptimizedAsync(int categoryId)
        {
            // دریافت تمام دسته‌بندی‌ها با روابط
            var allCategories = await _context.CatalogTypes
                .Include(c => c.Children)
                .ToListAsync();

            var targetCategory = allCategories.FirstOrDefault(c => c.Id == categoryId);
            if (targetCategory == null)
                return new List<CatalogTypeDto>();

            var result = new List<CatalogType> { targetCategory };
            var queue = new Queue<CatalogType>();
            queue.Enqueue(targetCategory);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var child in current.Children)
                {
                    if (!result.Contains(child))
                    {
                        result.Add(child);
                        queue.Enqueue(child);
                    }
                }
            }
            return _mapper.Map<List<CatalogTypeDto>>(result);
        }
    }
}