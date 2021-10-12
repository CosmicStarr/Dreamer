using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Interfaces;
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

        public TokenService(IConfiguration Config)
        {
            _config = Config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
        }

        public string Token(LoginDTO loginModelDTO, IList<string> Roles = null, IList<Claim> claims = null)
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
    }
}