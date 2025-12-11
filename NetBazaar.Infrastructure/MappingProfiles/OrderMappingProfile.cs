using AutoMapper;
using NetBazaar.Domain.Entities.Orders;
using System;
using NetBazaar.Domain.Extensions;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Infrastructure.MappingProfiles
{
    using AutoMapper;
    using global::NetBazaar.Application.DTOs.Order;

    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.AddressText, opt => opt.MapFrom(src => src.Address.AddressText))
                .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.Address.ReceiverName))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToPersianString()))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToPersianString()))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.ToPersianString()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}
