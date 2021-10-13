using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;
using Models.DTOS.OrderDTO;
using Models.Orders;
using NormStarr.ErrorHandling;
using NormStarr.Extensions;

namespace NormStarr.Controllers
{
    
    public class OrderController : BaseController
    {
        private readonly IOrderRepository _order;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public OrderController(IOrderRepository order, IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _order = order;

        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<ActualOrderDTO>> GetOrderAsync(int Id)
        {
            var Email = HttpContext.User.RetrieveUserEmail();
            var SingleObj = await _unitOfWork.Repository<ActualOrder>().GetFirstOrDefault(x =>x.ActualOrderId == Id,"OrderedItems,SpeaiclDelivery,ShippingAddress");
            if(SingleObj == null) return NotFound(new ApiErrorResponse(404));
            return Ok(_mapper.Map<ActualOrder,ActualOrderDTO>(SingleObj));
        }
        
     
        [HttpGet]
        // [Authorize]
        public async Task<ActionResult<IEnumerable<ActualOrderDTO>>> GetActualOrders()
        {
            var Email = HttpContext.User.RetrieveUserEmail();
            var Orders = await _unitOfWork.Repository<ActualOrder>().GetAll(x =>x.Email == Email,x =>x.OrderByDescending(x =>x.OrderDate)
            ,"OrderedItems,SpeaiclDelivery,ShippingAddress");
            return Ok(_mapper.Map<IEnumerable<ActualOrder>,IEnumerable<ActualOrderDTO>>(Orders));
        }

        [HttpGet("delivery")]
        public async Task<ActionResult<IEnumerable<DeliveryMethods>>> GetSpecialDeliveries()
        {
            return Ok(await _order.GetSpecialDeliveries());
        }
   
        [HttpPost]
        // [Authorize]
        public async Task<ActionResult<ActualOrder>> CreateOrder(OrderDTO orderDTO)
        {
            var Email = HttpContext.User.RetrieveUserEmail();
            var Address = _mapper.Map<AddressDTO, Address>(orderDTO.ShiptoAddress);
            var Order = await _order.CreateOrderAsync(Email, orderDTO.SpecialDeliveryID, orderDTO.CartId,Address);
            if (Order == null) return BadRequest(new ApiErrorResponse(400, "Problem Creating Order!"));
            return Ok(Order);
        }
    }
}