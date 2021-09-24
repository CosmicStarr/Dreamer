using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace NormStarr.Extensions
{
    public static class UserExtensions
    {
        public async static Task<AppUser> RetrieveEmail(this UserManager<AppUser> Input, ClaimsPrincipal User)
        {
            var currentUser =  User.FindFirstValue(ClaimTypes.Email);
            return await Input.Users.SingleOrDefaultAsync(x => x.Email == currentUser);
        }
    }
}