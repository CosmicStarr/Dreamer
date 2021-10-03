
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Data.Interfaces;
using Models;
using StackExchange.Redis;

namespace Data.ClassesForInterfaces
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IDatabase _database;
        public ShoppingCartRepository(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase();
        }
        public async Task<bool> DeleteCartAsync(string CartId)
        {
            return await _database.KeyDeleteAsync(CartId);
        }

        public async Task<ShoppingCart> GetCartAsync(string CartId)
        {
            var CartData = await _database.StringGetAsync(CartId);
            return CartData.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(CartData);
        }

        public async Task<ShoppingCart> UpdateCartAsync(ShoppingCart Cart)
        {
            var UpdatedCart = await _database.StringSetAsync(Cart.ShopId,JsonSerializer.Serialize(Cart),TimeSpan.FromDays(12));
            if(!UpdatedCart) return null;
            return await GetCartAsync(Cart.ShopId);
        }
    }
}