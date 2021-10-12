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
        Task<IdentityResult> SignUp(RegisterDTO registerDTO);
        Task<LoginDTO> Login(LoginDTO loginDTO);
        // Task<IdentityResult> ResetPassword(LoginDTO loginDTO);
        // Task<AppUser> ForgotPassword(LoginDTO loginDTO);
        Task<IdentityResult> ConfirmEmail(ConfirmEmailModelDTO confirmEmailModelDTO);
        string Token(LoginDTO loginModelDTO, IList<string> Roles, IList<Claim> claims);

    }
}