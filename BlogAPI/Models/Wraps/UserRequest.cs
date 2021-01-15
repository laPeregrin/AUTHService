using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Models.Wraps
{
    public class UserRequest
    {
        [Required]
        public string MessageRequest { get; set; }

    }
}
