using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SilverLinkAPI.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
        public string SilverUserId { get; set; }
        public virtual SilverUser SentBy { get; set; }
    }

    [Table("GroupMessages")]
    public class GroupMessage : Message
    {
        public int GroupId { get; set; }
    }

    [Table("FriendMessages")]
    public class FriendMessage : Message
    {
        public int FriendId { get; set; }
    }
}