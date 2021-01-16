using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUTHENTICATIONService.Models.Wraps
{
    public class RegisterAdminRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required, MinLength(5)]
        public string Password { get; set; }
        [Required, MinLength(5)]
        public string ConfirmPassword { get; set; }
    }
}
