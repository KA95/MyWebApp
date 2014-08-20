namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TagProblems", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagProblems", new[] { "Tag_Id" });
            DropPrimaryKey("dbo.Tags");
            DropPrimaryKey("dbo.TagProblems");
            AlterColumn("dbo.Tags", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TagProblems", "Tag_Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Tags", "Id");
            AddPrimaryKey("dbo.TagProblems", new[] { "Tag_Id", "Problem_Id" });
            CreateIndex("dbo.TagProblems", "Tag_Id");
            AddForeignKey("dbo.TagProblems", "Tag_Id", "dbo.Tags", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagProblems", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagProblems", new[] { "Tag_Id" });
            DropPrimaryKey("dbo.TagProblems");
            DropPrimaryKey("dbo.Tags");
            AlterColumn("dbo.TagProblems", "Tag_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Tags", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.TagProblems", new[] { "Tag_Id", "Problem_Id" });
            AddPrimaryKey("dbo.Tags", "Id");
            CreateIndex("dbo.TagProblems", "Tag_Id");
            AddForeignKey("dbo.TagProblems", "Tag_Id", "dbo.Tags", "Id", cascadeDelete: true);
        }
    }
}
