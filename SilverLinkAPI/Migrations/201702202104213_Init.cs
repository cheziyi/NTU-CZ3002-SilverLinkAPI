namespace SilverLinkAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        ProfilePicture = c.Binary(),
                        Role = c.Int(nullable: false),
                        DeviceId = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageText = c.String(),
                        MessageData = c.Binary(),
                        Type = c.Int(nullable: false),
                        SentAt = c.DateTime(nullable: false),
                        SilverUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SilverUsers", t => t.SilverUserId)
                .Index(t => t.SilverUserId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AcquiredAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PanicEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Description = c.String(),
                        ActivatedAt = c.DateTime(nullable: false),
                        ResolvedAt = c.DateTime(),
                        Location_Id = c.Int(),
                        SilverUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.SilverUsers", t => t.SilverUser_Id)
                .Index(t => t.Location_Id)
                .Index(t => t.SilverUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId1 = c.String(nullable: false, maxLength: 128),
                        UserId2 = c.String(nullable: false, maxLength: 128),
                        RequestedAt = c.DateTime(nullable: false),
                        AcceptedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SilverUsers", t => t.UserId1)
                .ForeignKey("dbo.SilverUsers", t => t.UserId2)
                .Index(t => t.UserId1)
                .Index(t => t.UserId2);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.GroupSilverUsers",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        SilverUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.SilverUser_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.SilverUsers", t => t.SilverUser_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.SilverUser_Id);
            
            CreateTable(
                "dbo.CarerUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Care_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.SilverUsers", t => t.Care_Id)
                .Index(t => t.Id)
                .Index(t => t.Care_Id);
            
            CreateTable(
                "dbo.SilverUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Location_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.GroupMessages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.FriendMessages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FriendId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.Id)
                .ForeignKey("dbo.Friends", t => t.FriendId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.FriendId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FriendMessages", "FriendId", "dbo.Friends");
            DropForeignKey("dbo.FriendMessages", "Id", "dbo.Messages");
            DropForeignKey("dbo.GroupMessages", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupMessages", "Id", "dbo.Messages");
            DropForeignKey("dbo.SilverUsers", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.SilverUsers", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CarerUsers", "Care_Id", "dbo.SilverUsers");
            DropForeignKey("dbo.CarerUsers", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Messages", "SilverUserId", "dbo.SilverUsers");
            DropForeignKey("dbo.Friends", "UserId2", "dbo.SilverUsers");
            DropForeignKey("dbo.Friends", "UserId1", "dbo.SilverUsers");
            DropForeignKey("dbo.PanicEvents", "SilverUser_Id", "dbo.SilverUsers");
            DropForeignKey("dbo.PanicEvents", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.GroupSilverUsers", "SilverUser_Id", "dbo.SilverUsers");
            DropForeignKey("dbo.GroupSilverUsers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.FriendMessages", new[] { "FriendId" });
            DropIndex("dbo.FriendMessages", new[] { "Id" });
            DropIndex("dbo.GroupMessages", new[] { "GroupId" });
            DropIndex("dbo.GroupMessages", new[] { "Id" });
            DropIndex("dbo.SilverUsers", new[] { "Location_Id" });
            DropIndex("dbo.SilverUsers", new[] { "Id" });
            DropIndex("dbo.CarerUsers", new[] { "Care_Id" });
            DropIndex("dbo.CarerUsers", new[] { "Id" });
            DropIndex("dbo.GroupSilverUsers", new[] { "SilverUser_Id" });
            DropIndex("dbo.GroupSilverUsers", new[] { "Group_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Friends", new[] { "UserId2" });
            DropIndex("dbo.Friends", new[] { "UserId1" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.PanicEvents", new[] { "SilverUser_Id" });
            DropIndex("dbo.PanicEvents", new[] { "Location_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "SilverUserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropTable("dbo.FriendMessages");
            DropTable("dbo.GroupMessages");
            DropTable("dbo.SilverUsers");
            DropTable("dbo.CarerUsers");
            DropTable("dbo.GroupSilverUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Friends");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.PanicEvents");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Locations");
            DropTable("dbo.Messages");
            DropTable("dbo.Groups");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
        }
    }
}
