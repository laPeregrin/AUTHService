using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUTHENTICATIONService.Models.Wraps
{
    public class AuthenticatedUserResponce
    {
        public string AccesToken { get; set; }
        public string RefreshToken { get; set; }
        public AuthenticatedUserResponce(string accesToken, string refreshToken)
        {
            AccesToken = accesToken;
            RefreshToken = refreshToken;
        }
    }
}
