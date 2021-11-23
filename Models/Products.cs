
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models
{
    public class Products
    {

        [Key]
        public int productId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public bool IsOnSale { get; set; } = false;
        public bool IsAvailable { get; set; } = false;
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public Photos Photos { get; set; } 
    }
}