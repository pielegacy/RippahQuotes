namespace RippahQuotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Effects : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "QuoteEffect", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quotes", "QuoteEffect");
        }
    }
}
