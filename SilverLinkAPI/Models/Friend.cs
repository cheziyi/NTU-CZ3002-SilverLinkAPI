using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilverLinkAPI.Models
{
    public class Friend
    {
        public int Id { get; set; }

        public string UserId1 { get; set; }

        public virtual SilverUser User { get; set; }

        public string UserId2 { get; set; }

        public virtual SilverUser UserFriend { get; set; }

        public DateTime RequestedAt { get; set; }

        public DateTime? AcceptedAt { get; set; }

        public ICollection<FriendMessage> Messages { get; set; }
    }
}