using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Orders;

namespace Data
{
    public class ApplicationDbContext:IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        
        public DbSet<AppUser> GetAppUsers { get; set; }
        public DbSet<Products> GetProducts { get; set; }
        public DbSet<Category> GetCategories { get; set; }
        public DbSet<Brand> GetBrands { get; set; }
        public DbSet<Address> GetAddresses { get; set; }
        public DbSet<ActualOrder> GetActualOrders { get; set; }
        public DbSet<CartItems> GetCartItems { get; set; }
        public DbSet<MappedProducts> GetMappedProducts { get; set; }
        public DbSet<OrderedItems> GetOrderedItems { get; set; }
        public DbSet<DeliveryMethods> GetDeliveryMethods { get; set; }
    }
}