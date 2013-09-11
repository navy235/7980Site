namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSenderIDForMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyMessage", "SenderID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyMessage", "SenderID");
        }
    }
}
