namespace SilverLinkAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinks : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PanicEvents", name: "SilverUser_Id", newName: "User_Id");
            RenameIndex(table: "dbo.PanicEvents", name: "IX_SilverUser_Id", newName: "IX_User_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PanicEvents", name: "IX_User_Id", newName: "IX_SilverUser_Id");
            RenameColumn(table: "dbo.PanicEvents", name: "User_Id", newName: "SilverUser_Id");
        }
    }
}
