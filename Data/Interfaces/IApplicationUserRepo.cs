using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.DTOS;

namespace Data.Interfaces
{
    public interface IApplicationUserRepo
    {
        Task<LoginDTO> SignUp(RegisterDTO registerDTO);
        Task<LoginDTO> Login(LoginDTO loginDTO);
        Task<IdentityResult> ResetPassword(ResetPassword loginDTO);
        Task<IdentityResult> ForgotPassword(ForgotPassword loginDTO);
        Task<IdentityResult> ConfirmEmail(ConfirmEmailModelDTO confirmEmailModelDTO);
        string LoginToken(LoginDTO loginModelDTO, IList<string> Roles, IList<Claim> claims);

    }
}