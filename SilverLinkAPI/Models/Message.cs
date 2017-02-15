using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilverLinkAPI.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
    }
}