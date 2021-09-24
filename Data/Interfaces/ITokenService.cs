using Models;

namespace Data.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser User);
    }
}