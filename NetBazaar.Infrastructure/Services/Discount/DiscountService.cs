using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.DTOs.Common;
using NetBazaar.Application.DTOs.Discounts;
using NetBazaar.Application.Interfaces;
using NetBazaar.Domain.Discounts;
using NetBazaar.Domain.Entities.Catalog;
using NetBazaar.Domain.Extensions;
using NetBazaar.Infrastructure.Data;
using NetBazaar.Persistence; // نام‌فضای DbContext پروژه
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetBazaar.Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly NetBazaarDbContext _context;
        private readonly IMapper _mapper;

        public DiscountService(NetBazaarDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateDiscountDto dto)
        {
            var discount = _mapper.Map<Discount>(dto);

            if (dto.CatalogItemIds != null && dto.CatalogItemIds.Any())
            {
                var items = await _context.CatalogItems
                    .Where(ci => dto.CatalogItemIds.Contains(ci.Id))
                    .ToListAsync();
                discount.CatalogItems = items;
            }

            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
            return discount.Id;
        }

        public async Task UpdateAsync(int id, CreateDiscountDto dto)
        {
            var discount = await _context.Discounts
                .Include(d => d.CatalogItems)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (discount == null) throw new KeyNotFoundException("تخفیف یافت نشد");

            discount.Name = dto.Name;
            discount.UsePercentage = dto.UsePercentage;
            discount.DiscountPercentage = dto.UsePercentage ? dto.DiscountPercentage : null;
            discount.DiscountAmount = dto.UsePercentage ? null : dto.DiscountAmount;
            discount.StartDate = dto.StartDate;
            discount.EndDate = dto.EndDate;
            discount.RequiresCouponCode = dto.RequiresCouponCode;
            discount.CouponCode = dto.CouponCode ?? string.Empty;
            discount.DiscountType = dto.DiscountType;
            discount.LimitationType = dto.LimitationType;
            discount.LimitationTimes = dto.LimitationTimes;

            if (dto.CatalogItemIds != null)
            {
                var items = await _context.CatalogItems
                    .Where(ci => dto.CatalogItemIds.Contains(ci.Id))
                    .ToListAsync();
                discount.CatalogItems = items;
            }
            else
            {
                discount.CatalogItems = new List<CatalogItem>();
            }

            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
        }

        public async Task<Discount?> GetByIdAsync(int id)
        {
            return await _context.Discounts
                .Include(d => d.CatalogItems)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<DiscountListItemDto>> GetAllAsync()
        {
            var discounts = await _context.Discounts
                            .OrderByDescending(d => d.StartDate).Select(d => _mapper.Map<DiscountListItemDto>(d))
                            .ToListAsync();
            return discounts;
        }

        public async Task DeleteAsync(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null) throw new KeyNotFoundException("تخفیف یافت نشد");
            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<DiscountListItemDto>> GetListAsync(int page, int pageSize)
        {
            var totalCount = await _context.Discounts.CountAsync();

            var query = _context.Discounts
             .OrderByDescending(d => d.StartDate).Select(d => _mapper.Map<DiscountListItemDto>(d))
                .AsNoTracking();
            var data = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            return new PaginatedResult<DiscountListItemDto>
            {
                Data = data,
                PageIndex = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}
