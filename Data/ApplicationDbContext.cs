using System.Reflection;
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
        public DbSet<ActualOrder> GetActualOrders { get; set; }
        public DbSet<MappedProducts> GetMappedProducts { get; set; }
        public DbSet<OrderedItems> GetOrderedItems { get; set; }
        // public DbSet<DeliveryMethods> GetDeliveryMethods { get; set; }
        public DbSet<UserAddress> GetUserAddresses { get; set; }
        public DbSet<Category> GetCategories { get; set; }
        public DbSet<Brand> GetBrands { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
 
        }
    }
}