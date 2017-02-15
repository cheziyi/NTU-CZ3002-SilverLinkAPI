namespace SilverLinkAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageText = c.String(),
                        SentAt = c.DateTime(nullable: false),
                        FriendId = c.Int(),
                        GroupId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Friend_Id = c.Int(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Friends", t => t.Friend_Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.Friend_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Descr = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupApplicationUsers",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.GroupApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupApplicationUsers", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Messages", "Friend_Id", "dbo.Friends");
            DropIndex("dbo.GroupApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GroupApplicationUsers", new[] { "Group_Id" });
            DropIndex("dbo.Messages", new[] { "Group_Id" });
            DropIndex("dbo.Messages", new[] { "Friend_Id" });
            DropTable("dbo.GroupApplicationUsers");
            DropTable("dbo.Groups");
            DropTable("dbo.Messages");
        }
    }
}
