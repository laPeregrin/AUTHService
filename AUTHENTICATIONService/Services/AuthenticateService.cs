


using AUTHENTICATIONService.Models.Wraps;
using DataBaseStaff.Models;

namespace AUTHENTICATIONService.Services
{
    public class AuthenticateService
    {
        private readonly AccessTokenGenerator AccessTokenGenerator;
        private readonly AccessTokenRefresh AccessTokenRefresh;

        public AuthenticateService(AccessTokenGenerator accessTokenGenerator, AccessTokenRefresh accessTokenRefresh)
        {
            AccessTokenGenerator = accessTokenGenerator;
            AccessTokenRefresh = accessTokenRefresh;
        }

        public AuthenticatedUserResponce Authenticate(User user)
        {
            string accessToken = AccessTokenGenerator.GenerateToken(user);
            string refreshToken = AccessTokenRefresh.GenerateToken(user.Id);

            return new AuthenticatedUserResponce(accessToken, refreshToken);
        }
    }
}
