using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilverLinkAPI.Models
{
    public class PanicEvent
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual Location Location { get; set; }
        public DateTime ActivatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}