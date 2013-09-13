namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OutDoorField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OutDoor", "IsRegular", c => c.Boolean(nullable: false));
            AddColumn("dbo.OutDoor", "TotalArea", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OutDoor", "TotalArea");
            DropColumn("dbo.OutDoor", "IsRegular");
        }
    }
}
