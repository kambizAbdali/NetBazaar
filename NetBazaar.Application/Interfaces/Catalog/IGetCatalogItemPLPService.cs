using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Catalog
{
    public interface IGetCatalogItemPLPService
    {
        PageResult<CatalogPLPDto> GetCatalogItems(
            IEnumerable<int>? categoryTypeIds,
            IEnumerable<int>? brandIds = null,
            long? minPrice = null,
            long? maxPrice = null,
            string sortBy = "newest",
            int page = 1,
            int pageSize = 12);
    }
}
