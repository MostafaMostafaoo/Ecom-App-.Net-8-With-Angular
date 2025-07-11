using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entity;
using Ecom.core.Entity.Order;

namespace Ecom.Api.Mapping
{
    public class OrderMapping: Profile
    {
        public OrderMapping()
        {
            CreateMap<Orders, OrderToReturnDTO>()
                
                .ForMember(d => d.deliveryMethod,o => 
                o.MapFrom(s => s.deliveryMethod.Name))
                .ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<ShippingAddress, ShipAddressDTO>().ReverseMap();
            CreateMap<Address, ShipAddressDTO>().ReverseMap();
            
        }
    }
}
