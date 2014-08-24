namespace MyWebApp.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class mig9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Videos", "ProblemId", "dbo.Problems");
            DropForeignKey("dbo.Videos", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Videos", new[] { "UserId" });
            DropIndex("dbo.Videos", new[] { "ProblemId" });
            DropTable("dbo.Videos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                        UserId = c.String(maxLength: 128),
                        ProblemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Videos", "ProblemId");
            CreateIndex("dbo.Videos", "UserId");
            AddForeignKey("dbo.Videos", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Videos", "ProblemId", "dbo.Problems", "Id", cascadeDelete: true);
        }
    }
}
