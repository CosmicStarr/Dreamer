using System;
using System.Linq;
using System.Security.Claims;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using StaticContent;

namespace Data.ClassesForInterfaces
{
    public class DataInitialize : IDataInitialize
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUser> _user;
        private readonly RoleManager<IdentityRole> _role;
        public DataInitialize(ApplicationDbContext db,UserManager<AppUser> user,RoleManager<IdentityRole> role)
        {
            _role = role;
            _user = user;
            _db = db;
            
        }
        public void Initialize()
        {
            try
            {
                if ( _db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

            if (_db.Roles.Any(r => r.Name == StaticInfo.AdminRole)) return;
            _role.CreateAsync(new IdentityRole(StaticInfo.AdminRole)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(StaticInfo.ManagerRole)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(StaticInfo.CustomerRole)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(StaticInfo.Employee)).GetAwaiter().GetResult();

            _user.CreateAsync(new AppUser
           {
                UserName = "Normanj85",
                Email = "Normanj85@yahoo.com",
                EmailConfirmed = true,
                FirstName = "Normand",
                LastName = "Jean",
           }, "Sonics123@1").GetAwaiter().GetResult();
           var AdminUser = _db.GetAppUsers.Where(x => x.Email == "Normanj85@yahoo.com").FirstOrDefault();
            _user.AddToRoleAsync(AdminUser, StaticInfo.AdminRole);
          
        }
    }
}