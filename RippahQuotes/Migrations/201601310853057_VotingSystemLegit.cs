namespace RippahQuotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VotingSystemLegit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteId = c.Int(nullable: false, identity: true),
                        QuoteId = c.Int(nullable: false),
                        IP = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.VoteId)
                .ForeignKey("dbo.Quotes", t => t.QuoteId, cascadeDelete: true)
                .Index(t => t.QuoteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "QuoteId", "dbo.Quotes");
            DropIndex("dbo.Votes", new[] { "QuoteId" });
            DropTable("dbo.Votes");
        }
    }
}
