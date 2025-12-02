using NetBazaar.Application.DTOs.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Catalog
{
    public interface IGetCatalogItemDetailService
    {
        CatalogDetailDto GetCatalogItemDetail(long id);
    }
}
