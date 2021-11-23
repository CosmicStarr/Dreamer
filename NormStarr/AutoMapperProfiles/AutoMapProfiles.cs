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
            CreateMap<Photos,PhotosDTO>().ReverseMap();
            CreateMap<ForgotPassword,ForgotPasswordDTO>().ReverseMap();
            CreateMap<ResetPassword,ResetPasswordDTO>().ReverseMap();
            CreateMap<AppUser,AppUserDTO>();
            CreateMap<OrderedItems,OrderedItemsDTO>();
            CreateMap<ActualOrder,ActualOrderDTO>().ForMember(x =>x.SpeaiclDelivery,o =>o.MapFrom(s =>s.SpeaiclDelivery.DeliveryId))
                                                   .ForMember(x => x.Total,o =>o.MapFrom(s =>s.GetTotal()))
                                                   .ForMember(x => x.ActualOrderId,o => o.MapFrom(s => s.ActualOrderId));                                                                                
            CreateMap<CartItems,CartItemsDTO>();
            CreateMap<ConfirmEmailModel,ConfirmEmailModelDTO>();
            CreateMap<UserAddress,UserAddressDTO>().ReverseMap().ForMember(d => d.UserAddressId,m => m.MapFrom(s =>s.UserAddressId));
            CreateMap<UserAddressDTO,Address>();                                        
            CreateMap<RegisterModel,RegisterDTO>();
            CreateMap<LoginModel,LoginDTO>().ReverseMap();
            CreateMap<Brand,BrandDTO>().ForMember(x =>x.Name,o =>o.MapFrom(s => s.Name));
            CreateMap<Category,CategoryDTO>().ForMember(x =>x.Name,o =>o.MapFrom(s => s.Name));
            
            CreateMap<PostProductsDTO,Products>().ReverseMap()
                                             .ForMember(x =>x.CategoryDTO,o =>o.MapFrom(s =>s.Category.CatId))
                                             .ForMember(x =>x.BrandDTO,o => o.MapFrom(s =>s.Brand.BrandId));

            CreateMap<Products,ProductsDTO>().ForMember(x => x.productsId,o =>o.MapFrom(s =>s.productId))
                                             .ForMember(x =>x.CategoryDTO, o =>o.MapFrom(s => s.Category.Name))
                                             .ForMember(x =>x.BrandDTO, o => o.MapFrom(s => s.Brand.Name))
                                             .ForMember(x =>x.photosDTO,o =>o.MapFrom(s =>s.Photos.PhotoUrl));
                                             
                                             
        }
    }
}