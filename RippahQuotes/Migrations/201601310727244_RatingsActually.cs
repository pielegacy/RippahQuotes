namespace RippahQuotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RatingsActually : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "QuoteRating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quotes", "QuoteRating");
        }
    }
}
