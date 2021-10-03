using AutoMapper;
using Models;
using Models.DTOS;
using Models.Orders;

namespace NormStarr.AutoMapperProfiles
{
    public class AutoMapProfiles:Profile
    {
        public AutoMapProfiles()
        {
            CreateMap<CartItems,CartItemsDTO>();
            CreateMap<ConfirmEmailModel,ConfirmEmailModelDTO>();
            CreateMap<Address,AddressDTO>();
            CreateMap<AppUser,AppUserDTO>();
            CreateMap<RegisterModel,RegisterDTO>().ReverseMap();
            CreateMap<LoginModel,LoginDTO>().ReverseMap();
            CreateMap<Brand,BrandDTO>().ForMember(x =>x.Name,o =>o.MapFrom(s => s.Name));
            CreateMap<Category,CategoryDTO>().ForMember(x =>x.Name,o =>o.MapFrom(s => s.Name));
            CreateMap<Products,ProductsDTO>().ForMember(x => x.ProductId,o =>o.MapFrom(s =>s.ProductId))
                                             .ForMember(x =>x.Category, o =>o.MapFrom(s => s.Category.Name))
                                             .ForMember(x =>x.Brand, o => o.MapFrom(s => s.Brand.Name));
        }
    }
}