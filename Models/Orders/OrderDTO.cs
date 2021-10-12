using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.DTOS;

namespace Models.Orders
{
    public class OrderDTO
    {
        public string CartId { get; set; }
        public string Email { get; set; }
        public int SpecialDeliveryID { get; set; }
        public int Amount { get; set; }
        public AddressDTO ShiptoAddress { get; set; }
    }
}