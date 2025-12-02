using AutoMapper;
using NetBazaar.Application.DTOs.Basket;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Domain.Entities.Basket;
using NetBazaar.Domain.Entities.Catalog;
using System;
using System.Linq;

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


            CreateMap<CatalogItem, CatalogPLPDto>()
           .ForMember(d => d.FirstImageSrc, opt => opt.MapFrom(s =>
               s.Images.OrderBy(i => i.SortOrder).Select(i => i.Src).FirstOrDefault()))
           .ForMember(d => d.Rating, opt => opt.MapFrom(s =>
               s.Ratings.Any() ? Math.Round(s.Ratings.Average(r => r.Score), 1) : 0))
           .ForMember(d => d.ReviewCount, opt => opt.MapFrom(s => s.Ratings.Count))
           .ForMember(d => d.IsFeatured, opt => opt.MapFrom(s =>
               s.Tags.Any(t => t.Value == "Featured")))
           .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.CatalogBrand.Brand)).ReverseMap();


            // مپینگ Basket به BasketDto
            CreateMap<Basket, BasketDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BuyerId, opt => opt.MapFrom(src => src.BuyerId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                //.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.GetTotalPrice()))
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.GetTotalItemsCount()));

            // مپینگ BasketItem به BasketItemDto
            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CatalogItemId, opt => opt.MapFrom(src => src.CatalogItemId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.GetTotalPrice()));

            // مپینگ معکوس اگر نیاز بود
            CreateMap<BasketDto, Basket>();
            CreateMap<BasketItemDto, BasketItem>();
        }
    }
}