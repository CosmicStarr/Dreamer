using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Models.DTOS;

namespace Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            
        }
        public ShoppingCart(string shopid)
        {
            shopid = ShopId;
        }
        [Key]
        public string ShopId { get; set; }
        public List<CartItemsDTO> ShoppingCartItems { get; set; } = new();
        public int? DeliveryId { get; set; }
        [Column(TypeName ="decimal (18,2)")]
        public decimal DeliveryPrice { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentID { get; set; }

    }
}