namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMediaFavorite : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Favorite",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberID = c.Int(nullable: false),
                        MediaID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Favorite");
        }
    }
}
