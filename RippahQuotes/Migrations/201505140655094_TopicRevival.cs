namespace RippahQuotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TopicRevival : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "TopicDescription", c => c.String(maxLength: 200));
            AddColumn("dbo.Topics", "TopicPassword", c => c.String(maxLength: 30));
            AlterColumn("dbo.Topics", "TopicName", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Topics", "TopicName", c => c.String(nullable: false));
            DropColumn("dbo.Topics", "TopicPassword");
            DropColumn("dbo.Topics", "TopicDescription");
        }
    }
}
