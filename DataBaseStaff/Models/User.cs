using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseStaff.Models
{
    public enum UserRole : byte
    {
        Reader = 0,
        Poster = 1,
        Admin = 2
    }

    public class domainObject 
    {
        [Required]
        public Guid Id { get; set; }
    };

    public class User : domainObject
    {
        
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        public UserRole Role { get; set; }
        [MinLength(5)]
        public string PasswordHash { get; set; }
        public ICollection<Publication>? Publications { get; set; }



    }
}
