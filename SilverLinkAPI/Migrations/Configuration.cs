namespace SilverLinkAPI.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SilverLinkAPI.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SilverLinkAPI.DAL.ApplicationDbContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            ApplicationUser user;

            user = new SilverUser { UserName = "92299962", PhoneNumber = "92299962", FullName = "Calvin Che", Role = 1, Location = new Location() { Latitude = 0, Longitude = 0, AcquiredAt = DateTime.UtcNow } };
            manager.Create(user, "Abcd!234");
            user = new SilverUser { UserName = "87654321", PhoneNumber = "87654321", FullName = "John Doe", Role = 1 };
            manager.Create(user, "Abcd!234");
            user = new CarerUser() { UserName = "81234567", PhoneNumber = "81234567", FullName = "Jane Doe", Role = 2 };
            manager.Create(user, "Abcd!234");
            user = new CarerUser() { UserName = "91234567", PhoneNumber = "91234567", FullName = "Janet Doe", Role = 2 };
            manager.Create(user, "Abcd!234");

            var friends = new List<Friend>
            {
                new Friend{ UserId1=manager.FindByName("92299962").Id, UserId2=manager.FindByName("87654321").Id,RequestedAt=DateTime.UtcNow, AcceptedAt=DateTime.UtcNow},
            };
            friends.ForEach(s => context.Friends.Add(s));
            context.SaveChanges();

            var friendMsgs = new List<Message>
            {
                new FriendMessage{FriendId=1,MessageText="Hello!",SilverUserId=manager.FindByName("92299962").Id, SentAt=DateTime.UtcNow},
                new FriendMessage{FriendId=1,MessageText="Hello to you too!",SilverUserId=manager.FindByName("87654321").Id, SentAt=DateTime.UtcNow},
            };
            friendMsgs.ForEach(s => context.Messages.Add(s));
            context.SaveChanges();

            var groups = new List<Group>
            {
                new Group{Name="Mahjong"},
                new Group{Name="Coffee"},
                new Group{Name="Strolling"},
                new Group{Name="Card Games"},
                new Group{Name="Board Games"},
                new Group{Name="Tai chi"},
            };
        }
    }
}
