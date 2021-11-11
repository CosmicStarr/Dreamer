using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Models;
using Models.DTOS;

namespace Data.Interfaces
{
    public interface ITokenService
    {
        string Token(AppUser loginModelDTO, IList<string> Roles = null, IList<Claim> claims = null);
        Task<string> CreateToken(AppUser appUser);
    }
}