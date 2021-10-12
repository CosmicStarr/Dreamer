using System.Collections.Generic;
using System.Security.Claims;
using Models;
using Models.DTOS;

namespace Data.Interfaces
{
    public interface ITokenService
    {
        string Token(LoginDTO loginModelDTO, IList<string> Roles = null, IList<Claim> claims = null);
    }
}