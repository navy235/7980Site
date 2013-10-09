namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "ArticleCodeValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "ArticleCodeValue");
        }
    }
}
