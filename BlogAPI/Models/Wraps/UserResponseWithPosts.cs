using DataBaseStaff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Models.Wraps
{
    public class ItemResponce
    {
        public string message { get; set; }
        public string messageId { get; set; }
    }

    public class UserResponseWithPosts
    {
        public List<ItemResponce> Publications { get; set; }

        public UserResponseWithPosts(IEnumerable<Publication> publications)
        {
            Publications = new List<ItemResponce>();

            foreach(var item in publications)
            {
                var responce = new ItemResponce();
                responce.message = item.Text;
                responce.messageId = item.Id.ToString();
                Publications.Add(responce);
            }
           
        }
    }
}
