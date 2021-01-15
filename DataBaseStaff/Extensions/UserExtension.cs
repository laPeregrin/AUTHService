using DataBaseStaff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseStaff.Extensions
{
    public static class UserExtension
    {
        public static ICollection<Publication> ReturnUpdateCollection(this User user, Publication publication)
        {
            var curlist = user.Publications;
            if (curlist == null)
            {
                var newList = new List<Publication>();
                newList.Add(publication);
                return newList;
            }
            curlist.Add(publication);
            return curlist;
        }
    }
}
