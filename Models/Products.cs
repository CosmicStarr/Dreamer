using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public bool IsOnSale { get; set; }
        public bool IsAvailable { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public string ImageUrl { get; set; }
    }
}