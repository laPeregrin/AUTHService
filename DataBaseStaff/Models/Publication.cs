using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseStaff.Models
{
    public class Publication : domainObject
    {
        public Publication(string text, User user)
        {
            Text = text;
            UserId = user.Id;
            User = user;
        }
        public Publication() { }

        [MaxLength(1000)]
        public string Text { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public IEnumerable<HashTag> HashTags { get; set; }

    }
}
