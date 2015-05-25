namespace RippahQuotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TopicCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "TopicAmount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Topics", "TopicAmount");
        }
    }
}
