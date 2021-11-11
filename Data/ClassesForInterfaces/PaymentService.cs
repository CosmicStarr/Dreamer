using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Models;
using Models.Orders;
using Stripe;

namespace Data.ClassesForInterfaces
{
    public class PaymentService : IPaymentService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public PaymentService(IShoppingCartRepository shoppingCartRepository, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _shoppingCartRepository = shoppingCartRepository;

        }
        public async Task<ShoppingCart> CreateOrUpdatePaymentIntent(string CartId)
        {
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            var Cart = await _shoppingCartRepository.GetCartAsync(CartId);
            if(Cart == null) return null;
            var shippingPrice = 0m;
            if(Cart.DeliveryId.HasValue)
            {
                var SpecialD = await _unitOfWork.Repository<DeliveryMethods>().Get((int)Cart.DeliveryId);
                shippingPrice = SpecialD.Price;
            }
            foreach (var item in Cart.ShoppingCartItems)
            {
                var obj = await _unitOfWork.Repository<Products>().Get(item.CartItemsId);
                if(item.Price != obj.Price)
                {
                    item.Price = obj.Price;
                }
            }
            var service = new PaymentIntentService();
            PaymentIntent intent;
            if(string.IsNullOrEmpty(Cart.PaymentID))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long) Cart.ShoppingCartItems.Sum(s => s.Amount * (s.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>{"card"} 
                };
                intent = await service.CreateAsync(options);
                Cart.PaymentID = intent.Id;
                Cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long) Cart.ShoppingCartItems.Sum(s => s.Amount * (s.Price * 100)) + (long)shippingPrice * 100,
                };
                await service.UpdateAsync(Cart.PaymentID,options);
            }

            await _shoppingCartRepository.UpdateCartAsync(Cart);
            return Cart;
        }

        public async Task<ActualOrder> UpdatePaymentFailed(string paymentId)
        {
            var Order = await _unitOfWork.Repository<ActualOrder>().GetFirstOrDefault(x =>x.PaymentId == paymentId);
            if(Order == null) return null;
            Order.Status = OrderStatus.PaymentFailed;
            _unitOfWork.Repository<ActualOrder>().Update(Order);
            await _unitOfWork.Complete();
            return Order;
        }

        public async Task<ActualOrder> UpdatePaymentSucceeded(string paymentId)
        {
            var Order = await _unitOfWork.Repository<ActualOrder>().GetFirstOrDefault(x =>x.PaymentId == paymentId);
            if(Order == null) return null;
            Order.Status = OrderStatus.PaymentRecevied;
            _unitOfWork.Repository<ActualOrder>().Update(Order);
            await _unitOfWork.Complete();
            return Order;
        }
    }
}