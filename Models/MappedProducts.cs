using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Models
{
    public class MappedProducts
    {
        public MappedProducts()
        {
            
        }
        public MappedProducts(int productsItemId, string itemName, string ImageUrl)
        {
            ProductsItemId = productsItemId;
            ItemName = itemName;
            photos = ImageUrl;
        }

        public int Id { get; set; }
        public int ProductsItemId { get; set; }
        public string ItemName { get; set; }
        public string photos { get; set; }
    }
}