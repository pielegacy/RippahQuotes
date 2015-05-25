namespace RippahQuotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Code_Password : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "QuotePassword", c => c.String(maxLength: 20));
            AlterColumn("dbo.Quotes", "QuoteAuthor", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotes", "QuoteAuthor", c => c.String(maxLength: 250));
            DropColumn("dbo.Quotes", "QuotePassword");
        }
    }
}
