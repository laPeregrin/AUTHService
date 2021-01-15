using AUTHENTICATIONService.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUTHENTICATIONService.Services.Implementations
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool isConfirmed(string HashedPassword, string password)
        {
          return BCrypt.Net.BCrypt.Verify(password, HashedPassword);
        }
    }
}
