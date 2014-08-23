namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Images", new[] { "UserId" });
            DropColumn("dbo.Images", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Images", "UserId");
            AddForeignKey("dbo.Images", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
