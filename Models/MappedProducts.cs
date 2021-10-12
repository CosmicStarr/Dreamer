using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class MappedProducts
    {
        public MappedProducts(int productsItemId, string itemName, string imageUrl)
        {
            ProductsItemId = productsItemId;
            ItemName = itemName;
            ImageUrl = imageUrl;
        }

        public int Id { get; set; }
        public int ProductsItemId { get; set; }
        public string ItemName { get; set; }
        public string ImageUrl { get; set; }
    }
}