namespace RippahQuotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quotes", "QuoteText", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Quotes", "QuoteAuthor", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotes", "QuoteAuthor", c => c.String(maxLength: 180));
            AlterColumn("dbo.Quotes", "QuoteText", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
