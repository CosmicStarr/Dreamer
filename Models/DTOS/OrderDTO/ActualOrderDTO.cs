using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTOS.OrderDTO
{
    public class ActualOrderDTO
    {
        public int ActualOrderId { get; set; }
        public string AppUser { get; set; }
        public string Email { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public AddressDTO ShippingAddress { get; set; }
        public string SpeaiclDelivery { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Subtotal { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Total { get; set; }
        public IEnumerable<OrderedItemsDTO> OrderedItems { get; set; }
        public string Status { get; set; } 
        public string PaymentId { get; set; }
    }
}