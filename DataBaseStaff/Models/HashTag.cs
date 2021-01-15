using System;
using System.Collections.Generic;

namespace DataBaseStaff.Models
{
    public class HashTag
    {
        public Guid Id { get; set; }
        public string HashTagContent { get; set; }
        public IEnumerable<Publication> Publications { get; set; }
    }
}
