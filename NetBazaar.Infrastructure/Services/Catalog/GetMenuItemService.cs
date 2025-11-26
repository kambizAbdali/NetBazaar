using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.Interfaces.Catalog;
using NetBazaar.Domain.Entities.Catalog;
using NetBazaar.Infrastructure.Data;
using NetBazaar.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace NetBazaar.Infrastructure.Services.Catalog
{
    public class GetMenuItemService : IGetMenuItemService
    {
        private readonly NetBazaarDbContext _context;
        private readonly IMapper _mapper;

        public GetMenuItemService(NetBazaarDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<MenuItemDto> GetMenuItems()
        {
            var catalogTypes = _context.CatalogTypes
                .Include(x => x.Children)
                    .ThenInclude(child => child.Children) // سطح دوم
                    .ThenInclude(grandChild => grandChild.Children) // سطح سوم 
                    .ThenInclude(grandChild => grandChild.Children) // سطح چهارم اگر نیاز است
                .AsNoTracking()
                .Where(x => x.ParentId == null)
                .ToList();

            return _mapper.Map<List<MenuItemDto>>(catalogTypes);
        }
    }
}
