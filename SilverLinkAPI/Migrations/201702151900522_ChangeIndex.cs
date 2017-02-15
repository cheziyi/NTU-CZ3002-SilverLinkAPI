namespace SilverLinkAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeIndex : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        PhoneNo = c.String(maxLength: 8, unicode: false),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.PhoneNo);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AppUsers", new[] { "PhoneNo" });
            DropTable("dbo.AppUsers");
        }
    }
}
