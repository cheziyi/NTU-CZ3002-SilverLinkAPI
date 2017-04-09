using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SilverLinkAPI.Models;

namespace SilverLinkAPI.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<SilverUser> SilverUsers { get; set; }
        public DbSet<CarerUser> CarerUsers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PanicEvent> PanicEvents { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Dobule foreign key on Friend for Users
            modelBuilder.Entity<Friend>()
                .HasRequired(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId1)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Friend>()
                .HasRequired(p => p.UserFriend)
                .WithMany()
                .HasForeignKey(p => p.UserId2)
                .WillCascadeOnDelete(false);
        }
    }
}