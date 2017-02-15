using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilverLinkAPI.Models
{
    public class Friend
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string FriendId { get; set; }

        public virtual ApplicationUser UserFriend { get; set; }

        public DateTime AddedDate { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}