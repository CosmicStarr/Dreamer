using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.Orders;

namespace Data.Interfaces
{
    public interface IPaymentService
    {
        Task<ShoppingCart> CreateOrUpdatePaymentIntent(string CartId);
        Task<ActualOrder> UpdatePaymentSucceeded(string paymentId);
        Task<ActualOrder> UpdatePaymentFailed(string paymentId);
    }
}