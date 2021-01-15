using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Models.Wraps
{
    public class UserResponce
    {
        public string Message { get; set; }

        public UserResponce(string message)
        {
            Message = message;
        }
    }
}
