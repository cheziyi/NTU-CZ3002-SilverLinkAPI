using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilverLinkAPI.Models
{
    public class Location
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime AcquiredAt { get; set; }
    }
}