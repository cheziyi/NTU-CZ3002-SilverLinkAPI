using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLinkAPI.Models
{
    public class Group
    {
        public int Id { get; set; }

        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public virtual ICollection<SilverUser> Members { get; set; }
        public virtual ICollection<GroupMessage> Messages { get; set; }
    }
}