using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOS;

namespace Data.ClassesForInterfaces
{
 public class TokenService:ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
        

        public TokenService(IConfiguration Config,UserManager<AppUser> userManager)
        {
            _config = Config;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
        }

        public string Token(AppUser loginModelDTO, IList<string> Roles = null, IList<Claim> claims = null)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.GivenName,loginModelDTO.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
            foreach (var item in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role.Remove(0,56),item));
            }
            //Secret Key for the Server to Use.
            var authKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:SecretKey"]));
            //Token Description
            var token = new JwtSecurityToken(
                    issuer: _config["JWT:VaildIssuer"],
                    audience: _config["JWT:VaildAudience"],
                    expires: DateTime.UtcNow.AddDays(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha512Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateToken(AppUser appUser)
        {
            //Adding Claims. Meaning whoever this Current user Claims to be. In this case the Username+.
            var ClaimsList = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.GivenName,appUser.Email),
               new Claim(JwtRegisteredClaimNames.Email,appUser.Email),
               new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };

            var roles = await _userManager.GetRolesAsync(appUser);
            ClaimsList.AddRange(roles.Select(x => new Claim(ClaimTypes.Role,x)));
            //Creating the Credentials for the current User.
             var authKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:SecretKey"]));
            //Describing the Token, first, sec, and third part of the token.
            var TokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(ClaimsList),
                Expires = DateTime.Now.AddDays(10),
                SigningCredentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha512Signature)
            };

            //Creating a token handler which will create the token
            var TokenHandler = new JwtSecurityTokenHandler();
            var Token = TokenHandler.CreateToken(TokenDescription);
            //returning the actually token.
            return TokenHandler.WriteToken(Token);
        }
    }
}