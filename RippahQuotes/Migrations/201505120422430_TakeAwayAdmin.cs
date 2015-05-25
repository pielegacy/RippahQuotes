namespace RippahQuotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TakeAwayAdmin : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Admins");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminID = c.Int(nullable: false, identity: true),
                        AdminName = c.String(nullable: false, maxLength: 20),
                        AdminPassword = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AdminID);
            
        }
    }
}
