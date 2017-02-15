using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilverLinkAPI.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descr { get; set; }
        public byte[] Image { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}