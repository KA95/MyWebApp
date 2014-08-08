namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Text = c.String(),
                        ProblemId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Problems", t => t.ProblemId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ProblemId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "ProblemId", "dbo.Problems");
            DropIndex("dbo.Answers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Answers", new[] { "ProblemId" });
            DropTable("dbo.Answers");
        }
    }
}
