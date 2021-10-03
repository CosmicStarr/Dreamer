using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTOS
{
    public class CartItemsDTO
    {
        public int CartItemsId { get; set; }
        public string ItemName { get; set; }
        [Column(TypeName ="decimal (18,2)")]
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string ImageUrl { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
    }
}