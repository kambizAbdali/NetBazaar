using AutoMapper;
using NetBazaar.Admin.EndPoint.ViewModels.Catalog;
using NetBazaar.Application.DTOs.Catalog;

namespace NetBazaar.Admin.EndPoint.Configuration
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile()
        {
            CreateMap<CatalogDto, CatalogViewModel>().ReverseMap();
            CreateMap<CatalogDto, CatalogDetailViewModel>().ReverseMap();
        }
    }
}
