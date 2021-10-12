using AutoMapper;
using Models;
using Models.DTOS;
using Models.DTOS.OrderDTO;
using Models.Orders;

namespace NormStarr.AutoMapperProfiles
{
    public class AutoMapProfiles:Profile
    {
        public AutoMapProfiles()
        {
            CreateMap<AppUser,AppUserDTO>();
            CreateMap<OrderedItems,OrderedItemsDTO>();
            CreateMap<ActualOrder,ActualOrderDTO>().ForMember(x =>x.SpeaiclDelivery,o =>o.MapFrom(s =>s.SpeaiclDelivery.DeliveryId))
                                                   .ForMember(x => x.Total,o =>o.MapFrom(s =>s.GetTotal()))
                                                   .ForMember(x => x.ActualOrderId,o => o.MapFrom(s => s.ActualOrderId));                               
            CreateMap<CartItems,CartItemsDTO>();
            CreateMap<ConfirmEmailModel,ConfirmEmailModelDTO>();
            CreateMap<Address,AddressDTO>().ReverseMap().ForMember(d => d.AddressId,m => m.MapFrom(s =>s.AddressId));
            CreateMap<RegisterModel,RegisterDTO>();
            CreateMap<LoginModel,LoginDTO>().ReverseMap();
            CreateMap<Brand,BrandDTO>().ForMember(x =>x.Name,o =>o.MapFrom(s => s.Name));
            CreateMap<Category,CategoryDTO>().ForMember(x =>x.Name,o =>o.MapFrom(s => s.Name));
            CreateMap<Products,ProductsDTO>().ForMember(x => x.ProductId,o =>o.MapFrom(s =>s.ProductId))
                                             .ForMember(x =>x.Category, o =>o.MapFrom(s => s.Category.Name))
                                             .ForMember(x =>x.Brand, o => o.MapFrom(s => s.Brand.Name));
        }
    }
}