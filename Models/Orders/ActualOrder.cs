using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Orders
{
    public class ActualOrder
    {
        public ActualOrder()
        {
            
        }
        public ActualOrder(
            IEnumerable<OrderedItems> orderedItems,
            string email,
            Address shippingAddress,  
            decimal subtotal,
            string paymentIntentId)
        {
            Email = email;
            ShippingAddress = shippingAddress;
            // SpeaiclDelivery = speaiclDelivery;
            Subtotal = subtotal;
            OrderedItems = orderedItems;
            PaymentId = paymentIntentId;
        }

        [Key]
        public int ActualOrderId { get; set; }
        public string Email { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShippingAddress { get; set; }
        // public DeliveryMethods SpeaiclDelivery { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Subtotal { get; set; }
        public IEnumerable<OrderedItems> OrderedItems { get; set; }
        public string OrderStatus { get; set; } 
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string PaymentId { get; set; }
        public decimal GetTotal()
        {
            return Subtotal;
        }
        // public decimal GetTotal()
        // {
        //     return Subtotal + SpeaiclDelivery.Price;
        // }

    }
}