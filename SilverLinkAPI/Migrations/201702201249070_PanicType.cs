namespace SilverLinkAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PanicType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PanicEvents", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PanicEvents", "Type");
        }
    }
}
