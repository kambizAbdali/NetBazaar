using AutoMapper;
using NetBazaar.Application.DTOs.Discounts;
using NetBazaar.Domain.Discounts;
using NetBazaar.Domain.Extensions;

namespace NetBazaar.Infrastructure.MappingProfiles
{
    public class DiscountMappingProfile : Profile
    {
        public DiscountMappingProfile()
        {
            CreateMap<CreateDiscountDto, Discount>()
                .ForMember(d => d.CatalogItems, opt => opt.Ignore()); // CatalogItems handled in service

            CreateMap<Discount, DiscountListItemDto>()
                    .ForMember(d => d.DiscountTypeString, opt => opt.MapFrom(src => src.DiscountType.GetDisplayName()))
                    .ForMember(d => d.LimitationTypeString, opt => opt.MapFrom(src => src.LimitationType.GetDisplayName()))
                .ReverseMap();
        }
    }
}
