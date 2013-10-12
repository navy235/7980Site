namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlertOpenIDlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Member", "OpenID", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Member", "OpenID", c => c.String(maxLength: 50));
        }
    }
}
