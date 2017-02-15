namespace SilverLinkAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUser : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AppUsers", new[] { "PhoneNo" });
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AddColumn("dbo.AspNetUsers", "ProfileImage", c => c.Binary());
            DropTable("dbo.AppUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        PhoneNo = c.String(maxLength: 8, unicode: false),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            DropColumn("dbo.AspNetUsers", "ProfileImage");
            DropColumn("dbo.AspNetUsers", "FullName");
            CreateIndex("dbo.AppUsers", "PhoneNo");
        }
    }
}
