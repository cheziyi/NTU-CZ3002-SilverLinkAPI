using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Collections.Generic;

namespace SilverLinkAPI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public string FullName { get; set; }
        public char Role { get; set; }
        public byte[] ProfilePicture { get; set; }

        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // public DbSet<UserProfile> UserProfiles { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friend>()
                .HasRequired(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Friend>()
                .HasRequired(p => p.UserFriend)
                .WithMany()
                .HasForeignKey(p => p.FriendId)
                .WillCascadeOnDelete(false);
        }
    }
}