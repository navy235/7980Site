namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMediaScheme : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Scheme",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberID = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Description = c.String(maxLength: 2000),
                        AddTime = c.DateTime(nullable: false),
                        LastTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.SchemeItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MediaID = c.Int(nullable: false),
                        SchemeID = c.Int(nullable: false),
                        PeriodCode = c.Int(nullable: false),
                        PeriodCount = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Scheme", t => t.SchemeID, cascadeDelete: true)
                .Index(t => t.SchemeID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SchemeItem", new[] { "SchemeID" });
            DropIndex("dbo.Scheme", new[] { "MemberID" });
            DropForeignKey("dbo.SchemeItem", "SchemeID", "dbo.Scheme");
            DropForeignKey("dbo.Scheme", "MemberID", "dbo.Member");
            DropTable("dbo.SchemeItem");
            DropTable("dbo.Scheme");
        }
    }
}
