using System.IO;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.Orders;
using NormStarr.ErrorHandling;
using Stripe;

namespace NormStarr.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private const string whSecret = ""
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

  
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePayment(string cartId)
        {
            var Cart = await _paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (Cart == null) return BadRequest(new ApiErrorResponse(400, "Your Cart Does Not Exist!"));
            return Ok(Cart);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], whSecret);
            PaymentIntent intent;
            ActualOrder actualOrder;
            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                intent = (PaymentIntent) stripeEvent.Data.Object;
                _logger.LogInformation("Payment Intent Succeeded",intent.Id);
                actualOrder = await _paymentService.UpdatePaymentSucceeded(intent.Id);
                _logger.LogInformation("Payment was a succeess!",intent.Id);
                break;
                case "payment_intent.payment_failed":
                intent = (PaymentIntent) stripeEvent.Data.Object;
                _logger.LogInformation("Payment Intent Failed",intent.Id);
                actualOrder = await _paymentService.UpdatePaymentFailed(intent.Id);
                _logger.LogInformation("Payment Failed!",intent.Id);
                break;
            }
            return new EmptyResult();
        }
    }
}
