using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AUTHENTICATIONService.Services
{
    public class GenerationToken
    {
        public string GenerateToken(string AccessToken,
            string Issuer,
            string Audience,
            double expirationMinutes,
            IEnumerable<Claim> claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccessToken));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new JwtSecurityToken(
                Issuer,
                Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(expirationMinutes),
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
