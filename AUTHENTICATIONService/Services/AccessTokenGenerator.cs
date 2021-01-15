using AUTHENTICATIONService.Models;
using AUTHENTICATIONService.Models.Wraps;
using Config;
using DataBaseStaff.Models;
using System.Collections.Generic;
using System.Security.Claims;


namespace AUTHENTICATIONService.Services
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfig _config;
        private readonly GenerationToken _generationToken;
        public AccessTokenGenerator(AuthenticationConfig config, GenerationToken generationToken)
        {
            _config = config;
            _generationToken = generationToken;
        }

        public string GenerateToken(User user)
        {

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };


            return _generationToken.GenerateToken(_config.AccessToken,
                _config.Issuer,
                _config.Audience,
                _config.AccessTokenExpirationMinutes,
                claims);
        }
    }
}
