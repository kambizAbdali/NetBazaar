using AutoMapper;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Domain.Entities.Catalog;

namespace NetBazaar.Infrastructure.MappingProfiles
{
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            // CatalogType mappings
            CreateMap<CatalogType, CatalogTypeDto>()
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children))
                .ReverseMap();

            CreateMap<CatalogType, CatalogTypeListDTO>()
                .ForMember(dest => dest.ChildrenCount, opt => opt.MapFrom(src => src.Children.Count));

            CreateMap<CatalogType, MenuItemDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.SubMenus, opt => opt.MapFrom(src => src.Children));

            // CatalogBrand mapping
            CreateMap<CatalogBrand, CatalogBrandDto>()
                .ReverseMap();

            // CatalogItem mappings
            CreateMap<CatalogItem, CatalogListDto>()
                .ForMember(d => d.TypeName, opt => opt.MapFrom(s => s.CatalogType.Type))
                .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.CatalogBrand.Brand))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => (decimal)s.Price));

            CreateMap<AddCatalogItemDto, CatalogItem>()
                .ForMember(d => d.CatalogBrandId, opt => opt.MapFrom(s => s.BrandId))
                .ForMember(d => d.Features, opt => opt.MapFrom(s => s.Features))
                .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images))
                .ReverseMap();

            CreateMap<CatalogItem, CatalogDto>()
                .ForMember(d => d.CatalogType, opt => opt.MapFrom(s => s.CatalogType))
                .ForMember(d => d.CatalogBrand, opt => opt.MapFrom(s => s.CatalogBrand))
                .ForMember(d => d.CatalogTypeId, opt => opt.MapFrom(s => s.CatalogTypeId))
                .ForMember(d => d.CatalogBrandId, opt => opt.MapFrom(s => s.CatalogBrandId))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => (decimal)s.Price))
                .ReverseMap();

            // Feature and Image mappings
            CreateMap<CatalogItemFeatureDto, CatalogItemFeature>()
                .ReverseMap();

            CreateMap<CatalogItemImageDto, CatalogItemImage>()
                .ReverseMap();
        }
    }
}