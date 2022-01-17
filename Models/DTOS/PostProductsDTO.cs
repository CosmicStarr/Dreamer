using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTOS
{
    public class PostProductsDTO
    {
        public int productsId { get; set; }
        [Required(ErrorMessage = "You must enter a name!")]
        [MaxLength(30,ErrorMessage ="Only 80 Characters allowed!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You must enter a description!")]
        [MaxLength(80,ErrorMessage ="Only 80 Characters allowed!")]
        public string Description { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        #nullable enable
        public bool? IsOnSale { get; set; } = false;
        public bool? IsAvailable { get; set; } = false;
        #nullable disable
        public string CategoryDTO { get; set; }
        public string BrandDTO { get; set; }
        public PhotosDTO photosDTO { get; set; }
    }
}