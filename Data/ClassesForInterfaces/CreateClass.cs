using AutoMapper;
using System.Threading.Tasks;
using Data.Interfaces;
using Models;
using Models.DTOS;

namespace Data.ClassesForInterfaces
{
    public class CreateClass : ICreateRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateClass(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            
        }
        public async Task<PostProductsDTO> AddEntityAsync(PostProductsDTO products)
        {
            products = new PostProductsDTO()
            {
                Name = products.Name,
                Description = products.Description,
                Price = products.Price,
                IsOnSale = products.IsOnSale,
                IsAvailable = products.IsAvailable,
                CategoryDTO = products.CategoryDTO,
                BrandDTO = products.BrandDTO
            };
            var obj = _mapper.Map<PostProductsDTO,Products>(products);
            _unitOfWork.Repository<Products>().Add(obj);
            await _unitOfWork.Complete();
            return products;
        }
    }
}