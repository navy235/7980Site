namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyMedia2 : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.OutDoor", "PeriodCode", "dbo.PeriodCate", "ID");
            CreateIndex("dbo.OutDoor", "PeriodCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OutDoor", new[] { "PeriodCode" });
            DropForeignKey("dbo.OutDoor", "PeriodCode", "dbo.PeriodCate");
        }
    }
}
