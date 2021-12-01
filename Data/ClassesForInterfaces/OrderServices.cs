using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Models;
using Models.Orders;

namespace Data.ClassesForInterfaces
{
    public class OrderServices : IOrderRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShoppingCartRepository _CartRepository;
        private readonly IPaymentService _paymentService;
        public OrderServices(IUnitOfWork unitOfWork, IShoppingCartRepository shoppingCartRepository, IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _CartRepository = shoppingCartRepository;
            _unitOfWork = unitOfWork;

        }
        public async Task<ActualOrder> CreateOrderAsync(string Email, int SpeciaDeliveryId, string CartId, Address address)
        {
            //get Shopping Cart
            var Cart = await _CartRepository.GetCartAsync(CartId);
            //get items in Shopping Cart
            var items = new List<OrderedItems>();
            foreach (var item in Cart.ShoppingCartItems)
            {
                var ProFromDb = await _unitOfWork.Repository<Products>().Get(item.CartItemsId);
                var Obj = new MappedProducts(ProFromDb.productId, ProFromDb.Name,item.PhotoUrl);
                var ItemsOrdered = new OrderedItems(Obj.ProductsItemId, Obj.ItemName, item.Price, item.Amount, Obj.ImageUrl);
                items.Add(ItemsOrdered);
            }
            var Delivery = await _unitOfWork.Repository<DeliveryMethods>().Get(SpeciaDeliveryId);
            var SubTotal = items.Sum(items => items.Price * items.Amount);
            var Order = new ActualOrder(items, Email, address, Delivery, SubTotal, Cart.PaymentID);
            //Adding Order To Database
            if (Order == null)
            {
                var existingOrder = await _unitOfWork.Repository<ActualOrder>().GetFirstOrDefault(x => x.PaymentId == Cart.PaymentID);
                if (existingOrder != null) _unitOfWork.Repository<ActualOrder>().Remove(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(Cart.PaymentID);
            }
            _unitOfWork.Repository<ActualOrder>().Add(Order);

            //Saving the Actually Order to the database!
            var results = await _unitOfWork.Complete();
            if (results <= 0) return null;

            // await _CartRepository.DeleteCartAsync(CartId);
            return Order;
        }

        public async Task<IEnumerable<DeliveryMethods>> GetSpecialDeliveries()
        {
            return await _unitOfWork.Repository<DeliveryMethods>().GetAll(null, x => x.OrderBy(x => x.Price));
        }
    }
}