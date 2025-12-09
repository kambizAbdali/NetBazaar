using AutoMapper;
using NetBazaar.Application.DTOs.Basket;
using NetBazaar.Application.DTOs.Catalog;
using NetBazaar.Domain.Entities.Basket;
using NetBazaar.Domain.Entities.Catalog;
using System;
using System.Linq;

namespace NetBazaar.Infrastructure.MappingProfiles
{
    using AutoMapper;
    using global::NetBazaar.Application.DTOs.User;
    using global::NetBazaar.Domain.Entities.Users;

    namespace NetBazaar.Infrastructure.MappingProfiles
    {
        public class AddressMappingProfile : Profile
        {
            public AddressMappingProfile()
            {
                // نگاشت از انتیتی به DTO برای نمایش
                CreateMap<UserAddress, UserAddressDto>();

                // نگاشت از DTO به انتیتی برای افزودن آدرس جدید
                CreateMap<CreateOrUpdateUserAddressDto, UserAddress>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore()) // چون EF مقداردهی می‌کند
                    .ForMember(dest => dest.UserId, opt => opt.Ignore()).ReverseMap(); // در سرویس مقداردهی می‌شود
            }
        }
    }

}