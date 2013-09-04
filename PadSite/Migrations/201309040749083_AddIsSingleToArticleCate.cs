namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsSingleToArticleCate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleCate", "IsSingle", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArticleCate", "IsSingle");
        }
    }
}
