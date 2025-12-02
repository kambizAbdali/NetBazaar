using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.DTOs.Common;
using NetBazaar.Application.Interfaces.Catalog;
using NetBazaar.Domain.Entities.Catalog;
using NetBazaar.Infrastructure.Services;
using NetBazaar.Infrastructure.Services.Catalog;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using System.Collections.Generic;
using System.Linq;

public class GetCatalogItemPLPService : IGetCatalogItemPLPService
{
    private readonly INetBazaarDbContext _context;
    private readonly IImageUrlService _imageService;
    private readonly IMapper _mapper;

    public GetCatalogItemPLPService(INetBazaarDbContext context, IImageUrlService imageService, IMapper mapper)
    {
        _context = context;
        _imageService = imageService;
        _mapper = mapper;
    }

    public PageResult<CatalogPLPDto> GetCatalogItems(
        IEnumerable<int>? categoryTypeIds,
        IEnumerable<int>? brandIds = null,
        long? minPrice = null,
        long? maxPrice = null,
        string sortBy = "newest",
        int page = 1,
        int pageSize = 12)
    {
        var query = _context.Catalogs
            .Include(x => x.Images)
            .Include(x => x.Ratings)
            .Include(x => x.Tags)
            .Include(x => x.CatalogBrand).AsQueryable();


        if (categoryTypeIds != null && categoryTypeIds.Any())
            query = query.Where(x => categoryTypeIds.Contains(x.CatalogTypeId));



        if (brandIds != null && brandIds.Any())
            query = query.Where(x => brandIds.Contains(x.CatalogBrandId));

        if (minPrice.HasValue)
            query = query.Where(x => x.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(x => x.Price <= maxPrice.Value);

        query = sortBy switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            "popular" => query.OrderByDescending(x => x.Ratings.Any() ? x.Ratings.Average(r => r.Score) : 0),
            _ => query.OrderByDescending(x => x.Id), // newest
        };

        var totalCount = query.Count();

        var entities = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = _mapper.Map<List<CatalogPLPDto>>(entities);

        //foreach (var d in dtos)
        //    d.FirstImageSrc = _imageService.GetFullImageUrl(d.FirstImageSrc);

        return new PageResult<CatalogPLPDto>(dtos, totalCount, page, pageSize);
    }
}
