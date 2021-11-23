using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
 
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }
        public string Name { get; set; }
    
    }
}