using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SilverLinkAPI.Models
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string MessageText { get; set; }
        [DataMember]
        public byte[] MessageData { get; set; }
        [DataMember]
        public MessageType Type { get; set; }
        [DataMember]
        public DateTime SentAt { get; set; }
        public string SilverUserId { get; set; }
        [DataMember]
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


    public enum MessageType
    {
        TextOnly = 0,
        Image = 1,
        Voice = 2
    }
}