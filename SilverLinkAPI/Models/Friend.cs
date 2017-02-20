using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SilverLinkAPI.Models
{
    [DataContract]
    public class Friend
    {
        [DataMember]
        public int Id { get; set; }

        public string UserId1 { get; set; }

        [DataMember]
        public virtual SilverUser User { get; set; }

        public string UserId2 { get; set; }
   
        public virtual SilverUser UserFriend { get; set; }

        [DataMember]
        public DateTime RequestedAt { get; set; }

        [DataMember]
        public DateTime? AcceptedAt { get; set; }

        [DataMember]
        public virtual ICollection<FriendMessage> Messages { get; set; }
    }
}