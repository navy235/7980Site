namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OutDoorMediaCodeValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OutDoor", "MediaCodeValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OutDoor", "MediaCodeValue");
        }
    }
}
