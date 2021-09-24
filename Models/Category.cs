using System.ComponentModel.DataAnnotations;

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