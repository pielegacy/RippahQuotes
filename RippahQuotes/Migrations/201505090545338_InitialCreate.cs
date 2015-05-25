namespace RippahQuotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quotes",
                c => new
                    {
                        QuoteId = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        QuoteText = c.String(),
                        QuoteAuthor = c.String(),
                    })
                .PrimaryKey(t => t.QuoteId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        TopicId = c.Int(nullable: false, identity: true),
                        TopicName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TopicId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quotes", "TopicId", "dbo.Topics");
            DropIndex("dbo.Quotes", new[] { "TopicId" });
            DropTable("dbo.Topics");
            DropTable("dbo.Quotes");
        }
    }
}
