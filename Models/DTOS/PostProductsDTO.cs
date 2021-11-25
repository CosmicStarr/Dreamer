using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTOS
{
    public class PostProductsDTO
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
        public PhotosDTO photosDTO { get; set; }
    }
}