using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
 
    public class Category
    {
        [Key]
        public int CatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
    }
}