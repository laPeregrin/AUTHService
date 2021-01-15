using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUTHENTICATIONService.Models.Wraps
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required ,MinLength(5)]
        public string Password { get; set; }
    }
}
