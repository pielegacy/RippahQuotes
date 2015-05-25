namespace RippahQuotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDB1 : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.Admins");
        }
    }
}
