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
   
    }
}