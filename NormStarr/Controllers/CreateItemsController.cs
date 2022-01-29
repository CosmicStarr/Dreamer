
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        // [Authorize(policy:"AdminManage")]
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

        // [Authorize(policy:"AdminManage")]
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
      
        // [Authorize(policy:"AdminManage")]
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<Products>> CreateProducts([FromBody]PostProductsDTO products)
        {
            //Create a Repository for the logic below
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
                        IsAvailable = products?.IsAvailable ??  false,
                        IsOnSale = products?.IsOnSale ??  false,
                        Category = await _unitOfWork.Repository<Category>().GetFirstOrDefault(x =>x.Name == products.CategoryDTO)??null,
                        Brand = await _unitOfWork.Repository<Brand>().GetFirstOrDefault(x =>x.Name == products.BrandDTO)??null
                    };  
                }
                _unitOfWork.Repository<Products>().Add(mapProduct);
            }
            await _unitOfWork.Complete();
            var mapProducts = _mapper.Map<Products,PostProductsDTO>(mapProduct);
            return Ok(mapProduct);
        }

        // [Authorize(policy:"AdminManage")]
        [HttpPut("UpdateProduct/{Id}", Name="GetProduct" )]
        public async Task<ActionResult<Products>> UpdateProduct([FromRoute]int Id, [FromBody]PostProductsDTO products)
        {   
            //Create a Repository for the logic below    
            var mapProduct = _mapper.Map<PostProductsDTO,Products>(products);
            mapProduct = await _unitOfWork.Repository<Products>().GetFirstOrDefault(x =>x.productId == Id);
            if(mapProduct != null)
            { 
                mapProduct.Name = products.Name;
                mapProduct.Description = products.Description;
                mapProduct.Price = products.Price;
                mapProduct.IsAvailable = products.IsAvailable ?? false;
                mapProduct.IsOnSale = products.IsOnSale ?? false;
                mapProduct.Category = await _unitOfWork.Repository<Category>().GetFirstOrDefault(x =>x.Name == products.CategoryDTO);
                mapProduct.Brand = await _unitOfWork.Repository<Brand>().GetFirstOrDefault(x =>x.Name == products.BrandDTO);
                mapProduct.Photos = await _unitOfWork.Repository<Photos>().GetFirstOrDefault(x =>x.ProductsId == products.productsId);                 
                _unitOfWork.Repository<Products>().Update(mapProduct);
            }
            await _unitOfWork.Complete();
            return Ok( _mapper.Map<Products,PostProductsDTO>(mapProduct));
        }

        // [Authorize(policy:"AdminManage")]
        [HttpDelete("DeleteProduct/{Id}")]
        public async Task DeleteProduct(int Id)
        {
            //Create a Repository for the logic below
            var objToDelete = await _unitOfWork.Repository<Products>().GetFirstOrDefault(x =>x.productId == Id);
            if(objToDelete != null)
            {
                objToDelete.Photos = await _unitOfWork.Repository<Photos>().GetFirstOrDefault(x =>x.ProductsId == Id);
                if(objToDelete.Photos != null)
                {
                    if(objToDelete.Photos.ProductsId != 0)
                    {
                        //deleting the photo in cloudinary first then database!
                        var results = await _photoServices.DeletePhotoAsync(objToDelete.Photos.PublicId);
                        _unitOfWork.Repository<Photos>().Remove(objToDelete.Photos);
                    }
                }
                _unitOfWork.Repository<Products>().Remove(objToDelete);
            }
            await _unitOfWork.Complete();
        }


        // [Authorize(policy:"AdminManage")]
        [HttpDelete("Delete-Photo/{Id}")]
        public async Task<ActionResult> DeletePhoto(int Id)
        {
            var obj = await _unitOfWork.Repository<Photos>().GetFirstOrDefault(x =>x.Id == Id);
            if(obj != null)
            {
                if(obj.PublicId is null)
                {
                    return NotFound();
                }
                else
                {
                    var results = await _photoServices.DeletePhotoAsync(obj.PublicId);
                    if(results.Error != null) return BadRequest(results.Error.Message);
                }
                _unitOfWork.Repository<Photos>().Remove(obj);
            }
            if(await _unitOfWork.Complete() > 0) return Ok();
            return BadRequest("Something went completely wrong! Failed to delete photo!");
        }


        // [Authorize(policy:"AdminManage")]
        [HttpPost("Add-Photo/{Id}")]
        public async Task<ActionResult<PhotosDTO>> CreatePhotos(int? Id, IFormFile file)
        {
            var obj = await _unitOfWork.Repository<Products>().GetFirstOrDefault(x => x.productId == Id,"Category,Brand,Photos");
            //results are coming from cloudinary!
            var results = await _photoServices.AddPhotoAsync(file);
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
    
            if(await _unitOfWork.Complete() > 0) 
            {   
                return CreatedAtRoute("GetProduct", new {Id = obj.productId}, _mapper.Map<Photos,PhotosDTO>(photo));
            }
            return BadRequest("Sorry! There was a problem Uploading your Photo!");
        }
    }
}