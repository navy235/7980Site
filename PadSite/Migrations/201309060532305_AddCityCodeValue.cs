namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCityCodeValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member_Profile", "CityCodeValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member_Profile", "CityCodeValue");
        }
    }
}
