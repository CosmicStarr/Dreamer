using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTOS
{
    public class ProductsDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public bool IsOnSale { get; set; }
        public bool IsAvailable { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
    }
}