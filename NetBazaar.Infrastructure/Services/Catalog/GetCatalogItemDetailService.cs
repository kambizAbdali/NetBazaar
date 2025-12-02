using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.Interfaces.Catalog;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBazaar.Infrastructure.Services.Catalog
{
    public class GetCatalogItemDetailService : IGetCatalogItemDetailService
    {
        private readonly INetBazaarDbContext _context;

        public GetCatalogItemDetailService(INetBazaarDbContext context)
        {
            _context = context;
        }

        public CatalogDetailDto GetCatalogItemDetail(long id)
        {
            var item = _context.Catalogs
                .Include(x => x.Images)
                .Include(x => x.Ratings)
                .Include(x => x.CatalogBrand)
                .Include(x => x.CatalogType)
                .Include(x => x.Features)
                .FirstOrDefault(x => x.Id == id);

            if (item == null) return null;

            return new CatalogDetailDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                DiscountPercent = item.DiscountPercent,
                Images = item.Images.OrderBy(i => i.SortOrder).Select(i => i.Src).ToList(),
                Rating = item.Ratings.Any() ? Math.Round(item.Ratings.Average(r => r.Score), 1) : 0,
                ReviewCount = item.Ratings.Count(),
                BrandName = item.CatalogBrand.Brand,
                CategoryName = item.CatalogType.Type,
                StockQuantity = item.StockQuantity,
                Features = item.Features.Select(f => $"{f.Key}: {f.Value}")
            };
        }
    }
}
