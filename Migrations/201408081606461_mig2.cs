namespace MyWebApp.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Answers", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Answers", "Name");
            DropColumn("dbo.Answers", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Answers", "Name", c => c.String());
            CreateIndex("dbo.Answers", "ApplicationUser_Id");
            AddForeignKey("dbo.Answers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
