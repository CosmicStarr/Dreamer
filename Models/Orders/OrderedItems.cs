using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Models.Orders
{
    public class OrderedItems
    {
        public OrderedItems()
        {
            
        }

        public OrderedItems(int itemsId, string productName, decimal price, int amount,string photoUrl)
        {
            ItemsId = itemsId;
            ProductName = productName;
            Price = price;
            Amount = amount;
            PhotoUrl = photoUrl;

        }

        [Key]
        public int Id { get; set; }
        public int ItemsId { get; set; }
        public string ProductName { get; set; }
        public string PhotoUrl { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public int  Amount { get; set; }
    }
}