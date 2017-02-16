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
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public virtual ICollection<SilverUser> Members { get; set; }
        public virtual ICollection<GroupMessage> Messages { get; set; }
    }
}