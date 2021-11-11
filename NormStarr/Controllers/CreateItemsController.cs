
using System;
using System.IO;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using NormStarr.ErrorHandling;

namespace NormStarr.Controllers
{
    public class CreateItemsController:BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoServices _photoServices;
        private readonly IWebHostEnvironment _host;
        public CreateItemsController(IUnitOfWork unitOfWork, IPhotoServices photoServices,IWebHostEnvironment host)
        {
            _host = host;
            _photoServices = photoServices;
            _unitOfWork = unitOfWork;
            
        }

        [HttpPost("CreateProducts")]
        public async Task<ActionResult<Products>> CreateProducts(Products products)
        {
            string webrootpath = _host.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if(products.ProductId == 0)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webrootpath, @"images");
                var extension = Path.GetExtension(files[0].FileName);
                using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                products.ImageUrl = @"\images\" + fileName + extension;
                _unitOfWork.Repository<Products>().Add(products);
            }
            else
            {
                var obj = await _unitOfWork.Repository<Products>().Get(products.ProductId);
                if(files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webrootpath, @"images");
                    var new_extension = Path.GetExtension(files[0].FileName);

                    var imagePath = Path.Combine(webrootpath, products.ImageUrl.TrimStart('\\'));
                    if(System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, fileName + new_extension), FileMode.Create))
                    {
                            files[0].CopyTo(filestream);
                    }
                    products.ImageUrl = @"\images\" + fileName + new_extension;
                }
                else
                {
                    products.ImageUrl = obj.ImageUrl;
                }
                _unitOfWork.Repository<Products>().Update(products);
            }
            await _unitOfWork.Complete();
            return Ok(products);
        }



        [HttpDelete("DeleteProducts")]
        public async Task<ActionResult<Products>> DeleteProducts(int Id)
        {
            var obj = await _unitOfWork.Repository<Products>().Get(Id);
            string webrootpath = _host.WebRootPath;
            var imagePath = Path.Combine(webrootpath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
             _unitOfWork.Repository<Products>().Remove(obj);
            await _unitOfWork.Complete();
            return Ok();
        }
        
        [HttpPost("AddPhoto")]
        public async Task<ActionResult<Photos>> CreatePhotos(IFormFile photos)
        {
            //results are coming from cloudinary!
            var results = await _photoServices.AddPhotoAsync(photos);
            if(results.Error != null) return BadRequest(new ApiErrorResponse(400));
            var photo = new Photos
            {
                PhotoUrl = results.SecureUrl.AbsoluteUri,
                PublicId = results.PublicId
            };
            _unitOfWork.Repository<Photos>().Add(photo);
            if(await _unitOfWork.Complete()> 0) return Ok();
            return BadRequest("Problem Uploading!");
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