using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Data.Pager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;
using NormStarr.Extensions;

namespace NormStarr.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("IsAvailable")]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> IsAvailableAsync([FromQuery]PageParams Param)
        {
            var isAvailable = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.IsAvailable == true,x=>x.OrderBy(x=>x.Name),"Category,Brand,Photos");
            var mappedIsAvailable = _mapper.Map<PagerList<Products>,PagerList<ProductsDTO>>(isAvailable);
            return Ok(mappedIsAvailable);
        }

        [HttpGet("OnSale")]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> IsOnSaleAsync([FromQuery]PageParams Param)
        {
            var onSale = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.IsOnSale == true,r => r.OrderBy(s =>s.Name),"Category,Brand,Photos");
            var mappedOnSale = _mapper.Map<PagerList<Products>,PagerList<ProductsDTO>>(onSale);
            return Ok(mappedOnSale);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ProductsDTO>> GetProductAsync(int Id)
        {
            var obj = await _unitOfWork.Repository<Products>().GetFirstOrDefault(x => x.productId == Id,"Category,Brand,Photos");
            var mappedObj = _mapper.Map<Products,ProductsDTO>(obj);
            return Ok(mappedObj);
        }

        [HttpGet]
        // [Authorize]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> GetProductsAsync([FromQuery]PageParams Param,string Search)
        {  
            var Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,null,x=>x.OrderBy(x =>x.Name),"Category,Brand,Photos");
            if(!string.IsNullOrEmpty(Param.Sort))
            {
                switch (Param.Sort)
                {
                    case "priceAsc": Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,null,x=>x.OrderBy(x=>x.Price),"Category,Brand,Photos");
                    break;
                    case "priceDsc": Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,null,x=>x.OrderByDescending(x=>x.Price),"Category,Brand,Photos");
                    break;
                    default: Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,null,x=>x.OrderBy(x =>x.Name),"Category,Brand,Photos"); break;
                }
            }
            if(!string.IsNullOrEmpty(Search))
            {
                Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.Name.ToLower().Contains(Search),x=>x.OrderBy(x =>x.Name),"Category,Brand,Photos");
                switch (Param.Sort)
                {
                    case "priceAsc": Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.Name.ToLower().Contains(Search),x=>x.OrderBy(x =>x.Price),"Category,Brand,Photos");
                    break;
                    case "priceDsc": Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.Name.ToLower().Contains(Search),x=>x.OrderByDescending(x =>x.Price),"Category,Brand,Photos");
                    break;
                }
            }
            if(Param.BrandId.HasValue)
            {
                Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x=>x.Brand.BrandId == Param.BrandId,o =>o.OrderBy(x =>x.Name),"Category,Brand,Photos"); 
                switch (Param.Sort)
                {
                    case "priceAsc": Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x=>x.Brand.BrandId == Param.BrandId,o =>o.OrderBy(x =>x.Price),"Category,Brand,Photos"); 
                    break;
                    case "priceDsc": Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x=>x.Brand.BrandId == Param.BrandId,o =>o.OrderByDescending(x =>x.Price),"Category,Brand,Photos");
                    break;
                }
            }
            if(Param.CatId.HasValue)
            {
                Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.Category.CatId == Param.CatId,o =>o.OrderBy(x=>x.Name),"Category,Brand,Photos");
                switch (Param.Sort)
                {
                    case "priceAsc": Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.Category.CatId == Param.CatId,o =>o.OrderBy(x=>x.Price),"Category,Brand,Photos");
                    break;
                    case "priceDsc":  Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.Category.CatId == Param.CatId,o =>o.OrderByDescending(x=>x.Price),"Category,Brand,Photos");
                    break;
                }
            }
            if(Param.BrandId.HasValue && Param.CatId.HasValue)
            {
                Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x=>x.Brand.BrandId == Param.BrandId & x.Category.CatId == Param.CatId,o =>o.OrderBy(x =>x.Name),"Category,Brand,Photos");
                switch (Param.Sort)
                {
                    case "priceAsc":  Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x=>x.Brand.BrandId == Param.BrandId & x.Category.CatId == Param.CatId,o =>o.OrderBy(x =>x.Price),"Category,Brand,Photos");
                    break;
                    case "priceDsc":  Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x=>x.Brand.BrandId == Param.BrandId & x.Category.CatId == Param.CatId,o =>o.OrderByDescending(x =>x.Price),"Category,Brand,Photos");
                    break;
                }
            }
            //PageSize is the amout of items per page!
            Response.AddPaginationHeader(Data.CurrentPage,Data.PageSize,Data.TotalCount,Data.TotalPages);
            var newData = _mapper.Map<PagerList<Products>,PagerList<ProductsDTO>>(Data);
            return Ok(newData);
        }

        [HttpGet("Category/{Id}")]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> ProductsByCat([FromQuery]PageParams Param, int Id)
        {
            var Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.Category.CatId == Id,x =>x.OrderByDescending(x =>x.Category.Name),"Category,Brand,Photos");
            Response.AddPaginationHeader(Data.CurrentPage,Data.PageSize,Data.TotalCount,Data.TotalPages);
            var newData = _mapper.Map<PagerList<Products>,PagerList<ProductsDTO>>(Data);
            return Ok(newData);
        }

        [HttpGet("Brand")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetBrands()
        {
            var obj = await _unitOfWork.Repository<Brand>().GetAll();
            var mappedObj = _mapper.Map<IEnumerable<Brand>,IEnumerable<BrandDTO>>(obj);
            return Ok(mappedObj);
        }

        [HttpGet("Category")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCatgory()
        {
            var obj = await _unitOfWork.Repository<Category>().GetAll();
            var mappedObj = _mapper.Map<IEnumerable<Category>,IEnumerable<CategoryDTO>>(obj);
            return Ok(mappedObj);
        }
    }
}