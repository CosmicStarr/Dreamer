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
            CreateMap<ConfirmEmailModel,ConfirmEmailModelDTO>();
            CreateMap<Address,AddressDTO>();
            CreateMap<AppUser,AppUserDTO>();
            CreateMap<RegisterModel,RegisterDTO>().ReverseMap();
            CreateMap<LoginModel,LoginDTO>().ReverseMap();
            CreateMap<Brand,BrandDTO>();
            CreateMap<Category,CategoryDTO>();
            CreateMap<Products,ProductsDTO>().ForMember(x =>x.Category, o =>o.MapFrom(s => s.Category.Name))
                                             .ForMember(x =>x.Brand, o => o.MapFrom(s => s.Brand.Name));
        }
    }
}