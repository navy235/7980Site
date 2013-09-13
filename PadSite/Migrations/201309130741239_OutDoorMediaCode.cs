namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OutDoorMediaCode : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.OutDoor", name: "MeidaCode", newName: "MediaCode");
            AddColumn("dbo.OutDoor", "IrRegularArea", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OutDoor", "IrRegularArea");
            RenameColumn(table: "dbo.OutDoor", name: "MediaCode", newName: "MeidaCode");
        }
    }
}
