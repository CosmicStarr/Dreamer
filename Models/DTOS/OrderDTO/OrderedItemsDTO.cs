using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTOS.OrderDTO
{
    public class OrderedItemsDTO
    {
        public int ItemsId { get; set; }
        public string ProductName { get; set; }
        public string PhotoUrl { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public int  Amount { get; set; }
    }
}