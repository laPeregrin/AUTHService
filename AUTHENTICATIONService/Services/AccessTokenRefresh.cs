using Config;
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
    public class AccessTokenRefresh
    {
        private readonly AuthenticationConfig _config;
        private readonly GenerationToken _generationToken;

        public AccessTokenRefresh(AuthenticationConfig config, GenerationToken generationToken)
        {
            _config = config;
            _generationToken = generationToken;
        }

        public string GenerateToken(Guid id)
        {
            List<Claim> claims = new List<Claim>()
            {
               new Claim("id", id.ToString())
            };
            return _generationToken.GenerateToken(_config.RefreshTokenSecret,
                  _config.Issuer,
                  _config.Audience,
                  _config.RefreshTokenExpirationMinutes,
                  claims);
        }
    }
}
