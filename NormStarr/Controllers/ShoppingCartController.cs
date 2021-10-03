using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;

namespace NormStarr.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartRepository _shopRepo;
        public ShoppingCartController(IShoppingCartRepository shopRepo)
        {
            _shopRepo = shopRepo;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartIdAsync(string Id)
        {
            var CartId = await _shopRepo.GetCartAsync(Id);
            return Ok(CartId ?? new ShoppingCart(Id));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpDateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            var Cart = await _shopRepo.UpdateCartAsync(shoppingCart);
            return Ok(Cart);
        }

        [HttpDelete]
        public async Task DeleteCartAsync(string id)
        {
             await _shopRepo.DeleteCartAsync(id);
        }
    }
}