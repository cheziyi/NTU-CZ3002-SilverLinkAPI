using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SilverLinkAPI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    [DataContract]
    public class ApplicationUser : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public byte[] ProfilePicture { get; set; }
        [DataMember]
        public UserRole Role { get; set; }
        [DataMember]
        public string DeviceId { get; set; }

        [DataMember]
        public override string Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }

        [DataMember]
        public override string PhoneNumber
        {
            get { return base.PhoneNumber; }
            set { base.PhoneNumber = value; }
        }

    }


    [Table("SilverUsers")]
    public class SilverUser : ApplicationUser
    {
        public virtual ICollection<Group> Groups { get; set; }
        [DataMember]
        public virtual Location Location { get; set; }
        public virtual ICollection<CarerUser> Carers { get; set; }
        public virtual ICollection<PanicEvent> PanicEvents { get; set; }

        [NotMapped]
        public ICollection<Friend> Friends { get; set; }
    }

    [Table("CarerUsers")]
    public class CarerUser : ApplicationUser
    {
        public virtual SilverUser Care { get; set; }
    }


    public enum UserRole
    {
        Silver = 1,
        Carer = 2
    }

}