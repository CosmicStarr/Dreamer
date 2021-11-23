
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;
using NormStarr.ErrorHandling;

namespace NormStarr.Controllers
{
    public class CreateItemsController:BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoServices _photoServices;
        private readonly IMapper _mapper;
        public CreateItemsController(IUnitOfWork unitOfWork, IPhotoServices photoServices,IMapper mapper)
        {
            
            _mapper = mapper;
            _photoServices = photoServices;
            _unitOfWork = unitOfWork;
            
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody]Category category)
        {
            if(category.CatId == 0)
            {

                _unitOfWork.Repository<Category>().Add(category);
            }
            else
            {
                _unitOfWork.Repository<Category>().Update(category);            
            }
            await _unitOfWork.Complete();
            var mappedProduct = _mapper.Map<Category,CategoryDTO>(category);
            return Ok(mappedProduct);
        }
        
        [HttpPost("CreateBrand")]
        public async Task<ActionResult<BrandDTO>> CreateBrand(Brand brand)
        {
            if(brand.BrandId == 0)
            {
                   _unitOfWork.Repository<Brand>().Add(brand);
            }
            else
            {
                _unitOfWork.Repository<Brand>().Update(brand);
             
            }
            await _unitOfWork.Complete();
            var mappedProduct = _mapper.Map<Brand,BrandDTO>(brand);
            return Ok(mappedProduct);
        }
      

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<Products>> CreateProducts([FromBody]PostProductsDTO products)
        {
            var mapProduct = _mapper.Map<PostProductsDTO,Products>(products);
            if(mapProduct.productId == 0)
            { 
                if(mapProduct.Category == null)
                {
                    mapProduct = new Products()
                    {
                        productId = products.productsId,
                        Name = products.Name,
                        Description = products.Description,
                        Price = products.Price,
                        IsAvailable = products.IsAvailable,
                        IsOnSale = products.IsOnSale,
                        Category = await _unitOfWork.Repository<Category>().GetFirstOrDefault(x =>x.Name == products.CategoryDTO),
                        Brand = await _unitOfWork.Repository<Brand>().GetFirstOrDefault(x =>x.Name == products.BrandDTO)
                    };  
                }
                _unitOfWork.Repository<Products>().Add(mapProduct);
            }
            await _unitOfWork.Complete();
            var mapProducts = _mapper.Map<Products,PostProductsDTO>(mapProduct);
            return Ok(mapProduct);
        }

        [HttpPut("UpdateProduct/{Id}", Name="GetProduct" )]
        public async Task<ActionResult<Products>> UpdateProduct(int Id, PostProductsDTO products)
        {
            //I have to create a real put method
            var mapProduct = _mapper.Map<PostProductsDTO,Products>(products);
            mapProduct = await _unitOfWork.Repository<Products>().GetFirstOrDefault(x =>x.productId == Id);
             _unitOfWork.Repository<Products>().Remove(mapProduct);
            if(mapProduct != null)
            {
                    mapProduct = new Products()
                    {
                        productId = Id,
                        Name = products.Name,
                        Description = products.Description,
                        Price = products.Price,
                        IsAvailable = products.IsAvailable,
                        IsOnSale = products.IsOnSale,
                        Category = await _unitOfWork.Repository<Category>().GetFirstOrDefault(x =>x.Name == products.CategoryDTO),
                        Brand = await _unitOfWork.Repository<Brand>().GetFirstOrDefault(x =>x.Name == products.BrandDTO)
                    };  
                _unitOfWork.Repository<Products>().Update(mapProduct);
            }
            await _unitOfWork.Complete();
            return Ok( _mapper.Map<Products,PostProductsDTO>(mapProduct));
        }

        [HttpPost("AddPhoto/{Id}")]
        public async Task<ActionResult<PhotosDTO>> CreatePhotos(IFormFile photos, int? Id)
        {
            var obj = await _unitOfWork.Repository<Products>().GetFirstOrDefault(x => x.productId == Id,"Category,Brand,Photos");
            //results are coming from cloudinary!
            var results = await _photoServices.AddPhotoAsync(photos);
            if(results.Error != null) return BadRequest(new ApiErrorResponse(400));
            var photo = new Photos
            {
                Products = obj,
                PhotoUrl = results.SecureUrl.AbsoluteUri,
                PublicId = results.PublicId
            };
                
            // if(obj.Photos.Count != 0)
            // {
            //     photo.IsMain = true;
            // }
            if(obj.Photos == null)
            {
                photo.IsMain = true;
            }

            // obj.Photos.Add(photo);

            _unitOfWork.Repository<Photos>().Add(photo);
            // await _unitOfWork.Complete();
            if(await _unitOfWork.Complete() > 0) 
            {   
                return CreatedAtRoute("GetProduct", new {Id = obj.productId}, _mapper.Map<Photos,PhotosDTO>(photo));
            }
            return BadRequest("Sorry! There was a problem Uploading your Photo!");
            // return Ok( _mapper.Map<PhotosDTO>(photo));
        }

    }
}