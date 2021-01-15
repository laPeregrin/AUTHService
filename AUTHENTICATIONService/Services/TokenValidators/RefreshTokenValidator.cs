using Config;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUTHENTICATIONService.Services.TokenValidators
{
    public class RefreshTokenValidator
    {
        private readonly AuthenticationConfig _config;

        public RefreshTokenValidator(AuthenticationConfig config)
        {
            _config = config;
        }

        public bool Validate(string refreshToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.RefreshTokenSecret)),
                ValidIssuer = _config.Issuer,
                ValidAudience = _config.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken Validatetoken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
