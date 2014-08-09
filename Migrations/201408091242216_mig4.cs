namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Problems", name: "Author_Id", newName: "AuthorId");
            RenameIndex(table: "dbo.Problems", name: "IX_Author_Id", newName: "IX_AuthorId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Problems", name: "IX_AuthorId", newName: "IX_Author_Id");
            RenameColumn(table: "dbo.Problems", name: "AuthorId", newName: "Author_Id");
        }
    }
}
