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
            int actualOrderId, 
            string email, 
            Address shippingAddress, 
            DeliveryMethods speaiclDelivery, 
            decimal subtotal, 
            OrderStatus orderStatus)
        {
            ActualOrderId = actualOrderId;
            Email = email;
            ShippingAddress = shippingAddress;
            SpeaiclDelivery = speaiclDelivery;
            Subtotal = subtotal;
            OrderedItems = orderedItems;
        }

        [Key]
        public int ActualOrderId { get; set; }
        public string Email { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShippingAddress { get; set; }
        public DeliveryMethods SpeaiclDelivery { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Subtotal { get; set; }
        public IEnumerable<OrderedItems> OrderedItems { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentId { get; set; }
        public decimal GetTotal()
        {
            return Subtotal + SpeaiclDelivery.Price;
        }

    }
}