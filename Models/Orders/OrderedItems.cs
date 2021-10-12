using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Orders
{
    public class OrderedItems
    {
        public OrderedItems()
        {
            
        }

        public OrderedItems(int itemsId, string productName, string imageUrl, decimal price, int amount)
        {
            ItemsId = itemsId;
            ProductName = productName;
            ImageUrl = imageUrl;
            Price = price;
            Amount = amount;
        }

        [Key]
        public int Id { get; set; }
        public int ItemsId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public int  Amount { get; set; }
    }
}