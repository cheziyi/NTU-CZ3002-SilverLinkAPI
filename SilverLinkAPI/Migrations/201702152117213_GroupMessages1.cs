namespace SilverLinkAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupMessages1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Messages", "FriendId");
            DropColumn("dbo.Messages", "GroupId");
            DropColumn("dbo.Messages", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Messages", "GroupId", c => c.Int());
            AddColumn("dbo.Messages", "FriendId", c => c.Int());
        }
    }
}
