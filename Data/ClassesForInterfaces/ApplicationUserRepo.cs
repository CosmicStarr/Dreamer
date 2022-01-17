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
        private readonly ITokenService _token;
     
        public ApplicationUserRepo(
            ITokenService token,
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IConfiguration configuration,
            RoleManager<IdentityRole> RoleManager,IMailJetEmailSender emailSender)
        {
            _token = token;
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
        
        //I need to check if the reset password is working!
        public async Task<IdentityResult> ResetPassword(ResetPassword loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            if(User == null) return null;
            var role = await _userManager.GetRolesAsync(User);
            IList<Claim> Claim = await _userManager.GetClaimsAsync(User); 
            var newUser = await _userManager.ResetPasswordAsync(User,Token(loginDTO,role,Claim),loginDTO.Password);
            if(newUser.Succeeded) return newUser;
            return null;
        }

        //I need to check if the forgot password method is working!
        public async Task<IdentityResult> ForgotPassword(ForgotPassword loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            if(User == null || !await _userManager.IsEmailConfirmedAsync(User)) return null;
            //Create send email logic!
            var FPassword = await _userManager.GeneratePasswordResetTokenAsync(User);
            
            var UriQuerybuilder = new UriBuilder(_configuration["ReturnPath:resetPassword"]);
            var uriQuery = HttpUtility.ParseQueryString(UriQuerybuilder.Query);
            uriQuery["token"] = FPassword;
            uriQuery["userId"] = User.Id;
            UriQuerybuilder.Query = uriQuery.ToString();

            var urlString = UriQuerybuilder.ToString();

            var EmailFromMySite = _configuration["ReturnPath:SenderEmail"];

            await _emailSender.SendEmail(EmailFromMySite,User.Email,"Reset Your Password",urlString);

            return IdentityResult.Success;
        }

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
            return new LoginDTO
            {
                Email = loginDTO.Email,
                Password = loginDTO.Password,
                token = LoginToken(loginDTO, role, Claim)   
            };
        }

        public async Task<LoginDTO> SignUp(RegisterDTO registerDTO)
        {
            var NewUser = new AppUser
            {
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                UserName = registerDTO.Email,
            };
            var results = await _userManager.CreateAsync(NewUser, registerDTO.Password);
            if(results.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(NewUser.Email);
                var Token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var UriQuerybuilder = new UriBuilder(_configuration["ReturnPath:confirmEmail"]);
                var uriQuery = HttpUtility.ParseQueryString(UriQuerybuilder.Query);
                uriQuery["token"] = Token;
                uriQuery["userId"] = user.Id;
                UriQuerybuilder.Query = uriQuery.ToString();

                var urlString = UriQuerybuilder.ToString();

                var EmailFromMySite = _configuration["ReturnPath:SenderEmail"];

                await _emailSender.SendEmail(EmailFromMySite,user.Email,"Confirm your email address",urlString);
                // if(!await _roleManager.RoleExistsAsync(StaticInfo.AdminRole))
                // {
                //     await _roleManager.CreateAsync(new IdentityRole(StaticInfo.AdminRole));
                //     await _roleManager.CreateAsync(new IdentityRole(StaticInfo.ManagerRole));
                //     await _roleManager.CreateAsync(new IdentityRole(StaticInfo.Employee));
                //     await _roleManager.CreateAsync(new IdentityRole(StaticInfo.CustomerRole));
                //     await _userManager.AddToRolesAsync(user,new[] {StaticInfo.AdminRole,StaticInfo.ManagerRole});
                //     var claim = new Claim("JobDepartment", registerDTO.JobDepartment);
                //     await _userManager.AddClaimAsync(user,claim);
                //     await _userManager.GetRolesAsync(user);
                //     await _userManager.GetClaimsAsync(user); 
                // }
                // if(registerDTO.Role == StaticInfo.AdminRole)
                // {
                //     await _userManager.AddToRolesAsync(user,new[] {StaticInfo.AdminRole,StaticInfo.ManagerRole});
                // }
                // else if(registerDTO.Role == StaticInfo.ManagerRole)
                // {
                //      await _userManager.AddToRoleAsync(user,StaticInfo.ManagerRole);
                // }
                // else
                // {
                //     if(registerDTO.Role == StaticInfo.Employee)
                //     {
                //         await _userManager.AddToRoleAsync(user,StaticInfo.Employee);
                //     }
                //     else if (registerDTO.Role != StaticInfo.AdminRole || registerDTO.Role != StaticInfo.ManagerRole || registerDTO.Role != StaticInfo.Employee )
                //     {
                //         await _userManager.AddToRoleAsync(user,StaticInfo.CustomerRole);
                //     }               
                // }  
                await _userManager.GetRolesAsync(user);
                await _userManager.GetClaimsAsync(user); 
            }
            if(!await _roleManager.RoleExistsAsync(registerDTO.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(registerDTO.Role));
                await _userManager.AddToRoleAsync(NewUser,registerDTO.Role);
            }
            if(registerDTO.Role != StaticInfo.AdminRole || registerDTO.Role != StaticInfo.ManagerRole || registerDTO.Role != StaticInfo.Employee )
            {
                await _userManager.AddToRoleAsync(NewUser,StaticInfo.CustomerRole);
            }
                var role = await _userManager.GetRolesAsync(NewUser);
                IList<Claim> Claim = await _userManager.GetClaimsAsync(NewUser); 
                return new LoginDTO
                {
                    Email = registerDTO.Email,
                    Password = registerDTO.Password,
                    token = _token.Token(NewUser,role,Claim) 
                };
        }
        public string LoginToken(LoginDTO loginModelDTO, IList<string> Roles = null, IList<Claim> claims = null)
        {

            claims.Add(new Claim(JwtRegisteredClaimNames.Email,loginModelDTO.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.GivenName,loginModelDTO.Email));
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
        private string Token(ResetPassword loginModelDTO, IList<string> Roles = null, IList<Claim> claims = null)
        {

            claims.Add(new Claim(JwtRegisteredClaimNames.Email,loginModelDTO.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.GivenName,loginModelDTO.Email));
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