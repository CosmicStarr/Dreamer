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
            var isAvailable = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.IsAvailable == true,x=>x.OrderBy(x=>x.Name),"Category,Brand");
            var mappedIsAvailable = _mapper.Map<PagerList<Products>,PagerList<ProductsDTO>>(isAvailable);
            return Ok(mappedIsAvailable);
        }

        [HttpGet("OnSale")]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> IsOnSaleAsync([FromQuery]PageParams Param)
        {
            var onSale = await _unitOfWork.Repository<Products>().GetAllParams(Param,x =>x.IsOnSale == true,r => r.OrderBy(s =>s.Name),"Category,Brand");
            var mappedOnSale = _mapper.Map<PagerList<Products>,PagerList<ProductsDTO>>(onSale);
            return Ok(mappedOnSale);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ProductsDTO>> GetProductAsync(int Id)
        {
            var obj = await _unitOfWork.Repository<Products>().GetFirstOrDefault(x => x.ProductId == Id,"Category,Brand");
            var mappedObj = _mapper.Map<Products,ProductsDTO>(obj);
            return Ok(mappedObj);
        }

        [HttpGet]
        // [Authorize(Policy="AdminDevelopers")]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> GetProductsAsync([FromQuery]PageParams Param)
        {
            var Data = await _unitOfWork.Repository<Products>().GetAllParams(Param,null,x=>x.OrderBy(x =>x.Name),"Category,Brand");
            //PageSize is the amout of items per page!
            Response.AddPaginationHeader(Data.CurrentPage,Data.PageSize,Data.TotalCount,Data.TotalPages);
            var newData = _mapper.Map<PagerList<Products>,PagerList<ProductsDTO>>(Data);
            return Ok(Data);
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