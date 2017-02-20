using Microsoft.AspNet.Identity.EntityFramework;
using SilverLinkAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SilverLinkAPI.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
   
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
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
                .HasForeignKey(p => p.UserId1)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Friend>()
                .HasRequired(p => p.UserFriend)
                .WithMany()
                .HasForeignKey(p => p.UserId2)
                .WillCascadeOnDelete(false);
        }
        public DbSet<SilverUser> SilverUsers { get; set; }
        public DbSet<CarerUser> CarerUsers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PanicEvent> PanicEvents { get; set; }
    }
}