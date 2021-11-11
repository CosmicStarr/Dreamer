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
        public int SpecialDeliveryID { get; set; }
        public UserAddressDTO ShiptoAddress { get; set; }
    }
}