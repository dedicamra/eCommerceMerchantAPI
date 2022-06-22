using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MerchantApp.Utilities
{
    public static class JWTGenerator
    {
        public static string GenerateuserToken(string username, string role)
        {
            var claims = new Claim[]
            {
                new Claim("Username", username),
                new Claim(ClaimTypes.Role, role)
            };
            return GenerateToken(claims, DateTime.UtcNow.AddDays(1));
        }

        private static string GenerateToken(Claim[] claims, DateTime expires)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.StaticConfig.GetSection("AppSettings:Token").Value));
            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                issuer: "mysite",
                audience: "mysite",
                expires:expires,
                claims: claims,
                signingCredentials: signInCred
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }
    }
}
