

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Pictures")]
    public class Photos
    {
        public int Id { get; set; }
        public bool IsMain { get; set; }
        public string PhotoUrl { get; set; }
        public string PublicId { get; set; }
        public int ProductsId { get; set; }
        public Products Products { get; set; }
    
    }
}