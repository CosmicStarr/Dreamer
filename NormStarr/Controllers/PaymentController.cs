using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NormStarr.ErrorHandling;

namespace NormStarr.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePayment(string cartId)
        {
            var Cart = await _paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (Cart == null) return BadRequest(new ApiErrorResponse(400, "Your Cart Does Not Exist!"));
            return Ok(Cart);
        }
    }
}