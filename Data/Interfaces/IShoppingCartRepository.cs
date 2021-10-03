using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTOS;

namespace Data.Interfaces
{
    public interface IShoppingCartRepository
    {

        Task<ShoppingCart> GetCartAsync(string CartId);
        Task<ShoppingCart> UpdateCartAsync(ShoppingCart Cart);
        Task<bool> DeleteCartAsync(string CartId);

    }
}