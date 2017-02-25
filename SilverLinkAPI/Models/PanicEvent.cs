using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SilverLinkAPI.Models
{
    [DataContract]
    public class PanicEvent
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public PanicType Type { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public virtual Location Location { get; set; }
        [DataMember]
        public DateTime ActivatedAt { get; set; }
        [DataMember]
        public DateTime? ResolvedAt { get; set; }
        public virtual SilverUser User { get; set; }
    }

    public enum PanicType
    {
        PanicButton = 1,
        FallEvent = 2
    }
}