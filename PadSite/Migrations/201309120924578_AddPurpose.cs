namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurpose : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurposeCate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CateName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OutDoor_PurposeCate",
                c => new
                    {
                        OutDoorID = c.Int(nullable: false),
                        PurposeCateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OutDoorID, t.PurposeCateID })
                .ForeignKey("dbo.OutDoor", t => t.OutDoorID, cascadeDelete: true)
                .ForeignKey("dbo.PurposeCate", t => t.PurposeCateID, cascadeDelete: true)
                .Index(t => t.OutDoorID)
                .Index(t => t.PurposeCateID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.OutDoor_PurposeCate", new[] { "PurposeCateID" });
            DropIndex("dbo.OutDoor_PurposeCate", new[] { "OutDoorID" });
            DropForeignKey("dbo.OutDoor_PurposeCate", "PurposeCateID", "dbo.PurposeCate");
            DropForeignKey("dbo.OutDoor_PurposeCate", "OutDoorID", "dbo.OutDoor");
            DropTable("dbo.OutDoor_PurposeCate");
            DropTable("dbo.PurposeCate");
        }
    }
}
