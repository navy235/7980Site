namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSuggestStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OutDoor", "SuggestStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OutDoor", "SuggestStatus");
        }
    }
}
