
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
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

        
        [HttpPost("AddPhoto/Id")]
        public async Task<ActionResult<PhotosDTO>> CreatePhotos(IFormFile photos, int? Id)
        {
            var obj = await _unitOfWork.Repository<Products>().GetFirstOrDefault(x => x.Id == Id,"Category,Brand,Photos");
            //results are coming from cloudinary!
            var results = await _photoServices.AddPhotoAsync(photos);
            if(results.Error != null) return BadRequest(new ApiErrorResponse(400));
            var photo = new Photos
            {
                Products = obj,
                PhotoUrl = results.SecureUrl.AbsoluteUri,
                PublicId = results.PublicId
            };
                
            if(obj.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            _unitOfWork.Repository<Photos>().Add(photo);
            await _unitOfWork.Complete();
            return Ok( _mapper.Map<PhotosDTO>(photo));
        }

        // [HttpPost("Category")] 
        // public async Task<ActionResult<Category>> CreateCategory(Category category)
        // {
        //     if(category.CatId == 0)
        //     {
        //         _unitOfWork.Repository<Category>().Add(category);
        //     }
        //     else
        //     {
        //         _unitOfWork.Repository<Category>().Update(category);
        //     }
        //     await _unitOfWork.Complete();
        //     return Ok(category);
        // }

        // [HttpDelete("DeleteCategory")]
        // public async Task<ActionResult<Category>> DeleteCatgory(int? Id)
        // {
        //     var obj = await _unitOfWork.Repository<Category>().GetFirstOrDefault(x => x.CatId == Id);
        //     if(Id == null)
        //     {
        //         return NotFound();
        //     }
        //     else
        //     {
        //         _unitOfWork.Repository<Category>().Remove(obj);
        //     }
        //    await _unitOfWork.Complete();
        //    return Ok();
        // }

        // [HttpPost("Brand")]
        // public async Task<ActionResult<Brand>> CreateBrand(Brand brand)
        // {
        //     if(brand.BrandId == 0)
        //     {
        //         _unitOfWork.Repository<Brand>().Add(brand);
        //     }
        //     else
        //     {
        //         _unitOfWork.Repository<Brand>().Update(brand);
        //     }
        //     await _unitOfWork.Complete();
        //     return Ok(brand);
        // }

        // [HttpDelete("DeleteBrand")]
        // public async Task<ActionResult<Brand>> DeleteBrand(int? Id)
        // {
        //     var obj = await _unitOfWork.Repository<Brand>().GetFirstOrDefault(x => x.BrandId == Id);
        //     if(Id == null)
        //     {
        //         return NotFound();
        //     }
        //     else
        //     {
        //         _unitOfWork.Repository<Brand>().Remove(obj);
        //     }
        //    await _unitOfWork.Complete();
        //    return Ok();
        // }
    }
}