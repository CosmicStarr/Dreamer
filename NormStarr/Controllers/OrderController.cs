using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Data.Pager;
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
            //I removed the Special delivery properties
            var Email = HttpContext.User.RetrieveUserEmail();
            var SingleObj = await _unitOfWork.Repository<ActualOrder>().GetFirstOrDefault(x =>x.ActualOrderId == Id,"OrderedItems,ShippingAddress");
            // var SingleObj = await _unitOfWork.Repository<ActualOrder>().GetFirstOrDefault(x =>x.ActualOrderId == Id,"OrderedItems,SpeaiclDelivery,ShippingAddress");
            if(SingleObj == null) return NotFound(new ApiErrorResponse(404,"What you're looking for does not exist!"));
            return Ok(_mapper.Map<ActualOrder,ActualOrderDTO>(SingleObj));
        }
        
     
        [HttpGet("User")]
        // [Authorize]
        public async Task<ActionResult<IEnumerable<ActualOrderDTO>>> GetActualOrders()
        {
             //I removed the Special delivery properties
            var Email = HttpContext.User.RetrieveUserEmail();
            // var Orders = await _unitOfWork.Repository<ActualOrder>().GetAll(x =>x.Email == Email,x =>x.OrderByDescending(x =>x.OrderDate)
            // ,"OrderedItems,SpeaiclDelivery,ShippingAddress");
            var Orders = await _unitOfWork.Repository<ActualOrder>().GetAll(x =>x.Email == Email,x =>x.OrderByDescending(x =>x.OrderDate)
            ,"OrderedItems,ShippingAddress");
            return Ok(_mapper.Map<IEnumerable<ActualOrder>,IEnumerable<ActualOrderDTO>>(Orders));
        }

        [HttpGet("OrderSort")]
        public async Task<ActionResult<IEnumerable<ActualOrderDTO>>> GetActualOrdersWithinTimeframe([FromQuery]PageParams pageParams,string Search)
        {
            var Orders = await _unitOfWork.Repository<ActualOrder>().GetAll(x =>x.OrderDate >= DateTimeOffset.Now.AddDays(-30),x=>x.OrderByDescending(x=>x.OrderDate),"OrderedItems,ShippingAddress");
            if(!string.IsNullOrEmpty(pageParams.Sort))
            {
                switch (pageParams.Sort)
                {
                    case "order3M": Orders = await _unitOfWork.Repository<ActualOrder>().GetAll(x =>x.OrderDate >= DateTimeOffset.Now.AddDays(-90),x=>x.OrderByDescending(x=>x.OrderDate),"OrderedItems,ShippingAddress");
                    break;
                    case "order6M": Orders = await _unitOfWork.Repository<ActualOrder>().GetAll(x =>x.OrderDate >= DateTimeOffset.Now.AddDays(-180),x=>x.OrderByDescending(x=>x.OrderDate),"OrderedItems,ShippingAddress");
                    break;
                    default: Orders = await _unitOfWork.Repository<ActualOrder>().GetAll(x =>x.OrderDate >= DateTimeOffset.Now.AddDays(-30),x=>x.OrderByDescending(x=>x.OrderDate),"OrderedItems,ShippingAddress"); break;
                }
            }
            if(!string.IsNullOrEmpty(Search))
            {
                Orders = await _unitOfWork.Repository<ActualOrder>().GetAll(x =>x.OrderedItems.Any(x=>x.ProductName.ToLower().Contains(Search)),x=>x.OrderByDescending(x=>x.OrderDate > DateTimeOffset.Now.AddDays(-30)));
                if(pageParams.Sort != null)
                {
                    switch (pageParams.Sort)
                    {
                        case "order3M": await _unitOfWork.Repository<ActualOrder>().GetAll(x =>x.OrderedItems.Any(x=>x.ProductName.ToLower().Contains(Search)),x=>x.OrderByDescending(x=>x.OrderDate > DateTimeOffset.Now.AddDays(-90)));
                        break;
                        case "order6M":await _unitOfWork.Repository<ActualOrder>().GetAll(x =>x.OrderedItems.Any(x=>x.ProductName.ToLower().Contains(Search)),x=>x.OrderByDescending(x=>x.OrderDate > DateTimeOffset.Now.AddDays(-180)));
                        break;
                    }
                }
                
            } 
            return Ok(_mapper.Map<IEnumerable<ActualOrder>,IEnumerable<ActualOrderDTO>>(Orders));
        }

        [HttpGet("OrderedProducts")]
        public async Task<ActionResult<IEnumerable<OrderedItems>>> GetListOfOrderedProducts(string Search)
        {
            var objItems = await _unitOfWork.Repository<Products>().GetAll(x =>x.Name.ToLower().Contains(Search),null,null);
            var objList = new List<OrderedItems>();
            if(!string.IsNullOrEmpty(Search))
            {
                foreach (var item in objItems)
                {
                    var items = await _unitOfWork.Repository<OrderedItems>().GetFirstOrDefault(x =>x.ProductName == item.Name);
                    objList.Add(items);
                }
            }
            return Ok(_mapper.Map<IEnumerable<OrderedItems>,IEnumerable<OrderedItemsDTO>>(objList));
        }

        [HttpGet]
        // [Authorize(policy:"AdminManage")]
        public async Task<ActionResult<IEnumerable<ActualOrderDTO>>> GetAllOrdersAsync()
        {
            var Orders = await _unitOfWork.Repository<ActualOrder>().GetAll(null,x =>x.OrderByDescending(x =>x.OrderDate)
            ,"OrderedItems,ShippingAddress");
            // var Orders = await _unitOfWork.Repository<ActualOrder>().GetAll(x=>x.OrderStatus == StaticContent.StaticInfo.StatusSubmitted || x.OrderStatus == StaticContent.StaticInfo.StatusinProcess,x =>x.OrderByDescending(x =>x.OrderDate)
            // ,"OrderedItems,SpeaiclDelivery,ShippingAddress");
            return Ok(_mapper.Map<IEnumerable<ActualOrder>,IEnumerable<ActualOrderDTO>>(Orders));
        }

        [HttpPost("inProcess")]
        public async Task OrderInProcess([FromBody]int id)
        {
            await _order.UpdateOrderStatus(id,StaticContent.StaticInfo.StatusinProcess);
            await _unitOfWork.Complete();
        }

        [HttpPost("ready")]
        public async Task OrderReady(int id)
        {
            await _order.UpdateOrderStatus(id,StaticContent.StaticInfo.StatusReady);
            await _unitOfWork.Complete();
        }

        [HttpPost("cancel")]
        public async Task OrderCancel(int id)
        {
            await _order.UpdateOrderStatus(id,StaticContent.StaticInfo.StatusCancelled);
            await _unitOfWork.Complete();
        }

        // [HttpGet("deli")]
        // public async Task<ActionResult<IEnumerable<DeliveryMethods>>> GetSpecialDeliveries()
        // {
        //     return Ok(await _order.GetSpecialDeliveries());
        // }
   
        [HttpPost]
        public async Task<ActionResult<ActualOrder>> CreateOrder(OrderDTO orderDTO)
        {
            var Email = HttpContext.User.RetrieveUserEmail();
            var Address = _mapper.Map<UserAddressDTO, Address>(orderDTO.ShiptoAddress);
            var Order = await _order.CreateOrderAsync(Email, orderDTO.CartId,Address);
            // Order.OrderTimeSpan = Order.OrderDate.UtcTicks - DateTime.Now.Second;
            if (Order == null) return BadRequest(new ApiErrorResponse(400, "Problem Creating Order!"));
            return Ok(Order);
        }
    }
}