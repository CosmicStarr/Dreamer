using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.Orders;

namespace Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<ActualOrder> CreateOrderAsync(string Email,int SpeciaDeliveryId,string CartId,Address address);
        Task<IEnumerable<DeliveryMethods>> GetSpecialDeliveries();
        
    }
}