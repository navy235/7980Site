namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmembermobile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "Mobile", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "Mobile");
        }
    }
}
