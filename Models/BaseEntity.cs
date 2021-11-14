
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}