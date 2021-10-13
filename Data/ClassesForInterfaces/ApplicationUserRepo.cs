using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOS;
using StaticContent;


namespace Data.ClassesForInterfaces
{
    public class ApplicationUserRepo : IApplicationUserRepo
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailJetEmailSender _emailSender;
     
        public ApplicationUserRepo(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IConfiguration configuration,
            RoleManager<IdentityRole> RoleManager,IMailJetEmailSender emailSender)
        {
            _emailSender = emailSender;
            _configuration = configuration;
            _roleManager = RoleManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> ConfirmEmail(ConfirmEmailModelDTO confirmEmailModelDTO)
        {
            var user = await _userManager.FindByIdAsync(confirmEmailModelDTO.userId);
            var results = await _userManager.ConfirmEmailAsync(user,confirmEmailModelDTO.token);
            if(results.Succeeded) return results;
            return null;
        }

        // public async Task<AppUser> ForgotPassword(LoginDTO loginDTO)
        // {
        //     var User = await _userManager.FindByEmailAsync(loginDTO.Email);
        //     if(User == null || !await _userManager.IsEmailConfirmedAsync(User)) return null;
        //     //Create send email logic!
        //     return User;
        // }

        public async Task<LoginDTO> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if(user == null) return null;
            var result = await _signInManager.PasswordSignInAsync(user.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return null;
            }
            var role = await _userManager.GetRolesAsync(user);
            IList<Claim> Claim = await _userManager.GetClaimsAsync(user); 
            
            //Who the Application User Claims to be!
            // return Token(new LoginDTO 
            // {
            //     Email = loginDTO.Email,
            //     Password = loginDTO.Password
            // },role,Claim);
            return new LoginDTO
            {
                Email = loginDTO.Email,
                Password = loginDTO.Password,
                token = Token(loginDTO, role, Claim)   
            };
        }

        // public async Task<IdentityResult> ResetPassword(LoginDTO loginDTO)
        // {
        //     var User = await _userManager.FindByEmailAsync(loginDTO.Email);
        //     if(User == null) return null;
        //     var newUser = await _userManager.ResetPasswordAsync(User,Token(loginDTO),loginDTO.Password);
        //     if(newUser.Succeeded) return newUser;
        //     return null;
        // }

        public async Task<IdentityResult> SignUp(RegisterDTO registerDTO)
        {

            if(!await _roleManager.RoleExistsAsync(registerDTO.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(registerDTO.Role));
            }
            var NewUser = new AppUser
            {
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                UserName = registerDTO.Email
            };
            var results = await _userManager.CreateAsync(NewUser, registerDTO.Password);
            if(results.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(NewUser.Email);
                var Token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var UriQuerybuilder = new UriBuilder(_configuration["ReturnPath:ConfirmEmail"]);
                var uriQuery = HttpUtility.ParseQueryString(UriQuerybuilder.Query);
                uriQuery["token"] = Token;
                uriQuery["userId"] = user.Id;
                UriQuerybuilder.Query = uriQuery.ToString();

                var urlString = UriQuerybuilder.ToString();

                var EmailFromMySite = _configuration["ReturnPath:SenderEmail"];

                await _emailSender.SendEmail(EmailFromMySite,user.Email,"Confirm your email address",urlString);

                var role = await _userManager.AddToRoleAsync(user,registerDTO.Role);
                if(role == null) 
                {
                    role = await _userManager.AddToRoleAsync(user,StaticInfo.CustomerRole);
                }
                var claim  = new Claim("JobDepartment",registerDTO.JobDepartment);
                await _userManager.AddClaimAsync(NewUser,claim);
                return IdentityResult.Success;
            }
            return IdentityResult.Failed();
        }

        public string Token(LoginDTO loginModelDTO, IList<string> Roles = null, IList<Claim> claims = null)
        {
            // var authClaims = new List<Claim>
            // {
            //     new Claim(ClaimTypes.Name,loginModelDTO.Email),
            //     new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            // };
            claims.Add(new Claim(JwtRegisteredClaimNames.GivenName,loginModelDTO.Email));
            claims.Add(new Claim(ClaimTypes.Email,loginModelDTO.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
            foreach (var item in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role.Remove(0,56),item));
            }
            //Secret Key for the Server to Use.
            var authKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]));
            //Token Description
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:VaildIssuer"],
                    audience: _configuration["JWT:VaildAudience"],
                    expires: DateTime.UtcNow.AddDays(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha512Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

 
    }
}