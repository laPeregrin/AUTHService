using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUTHENTICATIONService.Services.Abstractions
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool isConfirmed(string HashedPassword, string password);
    }
}
