using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTOS
{
    public class ProductsDTO
    {
        public int productsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public bool IsOnSale { get; set; } = false;
        public bool IsAvailable { get; set; } = false;
        public string CategoryDTO { get; set; }
        public string BrandDTO { get; set; }
        public string photosDTO { get; set; }
    }
}