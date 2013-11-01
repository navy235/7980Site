namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyMedia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OutDoor", "PeriodCode", "dbo.PeriodCate");
            DropForeignKey("dbo.OutDoor", "OwnerCode", "dbo.OwnerCate");
            DropForeignKey("dbo.OutDoor_AreaCate", "OutDoorID", "dbo.OutDoor");
            DropForeignKey("dbo.OutDoor_AreaCate", "AreaCateID", "dbo.AreaCate");
            DropForeignKey("dbo.OutDoor_CrowdCate", "OutDoorID", "dbo.OutDoor");
            DropForeignKey("dbo.OutDoor_CrowdCate", "CrowdCateID", "dbo.CrowdCate");
            DropForeignKey("dbo.OutDoor_IndustryCate", "OutDoorID", "dbo.OutDoor");
            DropForeignKey("dbo.OutDoor_IndustryCate", "IndustryCateID", "dbo.IndustryCate");
            DropForeignKey("dbo.OutDoor_PurposeCate", "OutDoorID", "dbo.OutDoor");
            DropForeignKey("dbo.OutDoor_PurposeCate", "PurposeCateID", "dbo.PurposeCate");
            DropIndex("dbo.OutDoor", new[] { "PeriodCode" });
            DropIndex("dbo.OutDoor", new[] { "OwnerCode" });
            DropIndex("dbo.OutDoor_AreaCate", new[] { "OutDoorID" });
            DropIndex("dbo.OutDoor_AreaCate", new[] { "AreaCateID" });
            DropIndex("dbo.OutDoor_CrowdCate", new[] { "OutDoorID" });
            DropIndex("dbo.OutDoor_CrowdCate", new[] { "CrowdCateID" });
            DropIndex("dbo.OutDoor_IndustryCate", new[] { "OutDoorID" });
            DropIndex("dbo.OutDoor_IndustryCate", new[] { "IndustryCateID" });
            DropIndex("dbo.OutDoor_PurposeCate", new[] { "OutDoorID" });
            DropIndex("dbo.OutDoor_PurposeCate", new[] { "PurposeCateID" });
            AddColumn("dbo.OutDoor", "RealPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Member", "MemberType", c => c.Int(nullable: false));
            DropTable("dbo.AreaCate");
            DropTable("dbo.CrowdCate");
            DropTable("dbo.IndustryCate");
            DropTable("dbo.PurposeCate");
            DropTable("dbo.OutDoor_AreaCate");
            DropTable("dbo.OutDoor_CrowdCate");
            DropTable("dbo.OutDoor_IndustryCate");
            DropTable("dbo.OutDoor_PurposeCate");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OutDoor_PurposeCate",
                c => new
                    {
                        OutDoorID = c.Int(nullable: false),
                        PurposeCateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OutDoorID, t.PurposeCateID });
            
            CreateTable(
                "dbo.OutDoor_IndustryCate",
                c => new
                    {
                        OutDoorID = c.Int(nullable: false),
                        IndustryCateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OutDoorID, t.IndustryCateID });
            
            CreateTable(
                "dbo.OutDoor_CrowdCate",
                c => new
                    {
                        OutDoorID = c.Int(nullable: false),
                        CrowdCateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OutDoorID, t.CrowdCateID });
            
            CreateTable(
                "dbo.OutDoor_AreaCate",
                c => new
                    {
                        OutDoorID = c.Int(nullable: false),
                        AreaCateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OutDoorID, t.AreaCateID });
            
            CreateTable(
                "dbo.PurposeCate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CateName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.IndustryCate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CateName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CrowdCate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CateName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AreaCate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CateName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.Member", "MemberType");
            DropColumn("dbo.OutDoor", "RealPrice");
            CreateIndex("dbo.OutDoor_PurposeCate", "PurposeCateID");
            CreateIndex("dbo.OutDoor_PurposeCate", "OutDoorID");
            CreateIndex("dbo.OutDoor_IndustryCate", "IndustryCateID");
            CreateIndex("dbo.OutDoor_IndustryCate", "OutDoorID");
            CreateIndex("dbo.OutDoor_CrowdCate", "CrowdCateID");
            CreateIndex("dbo.OutDoor_CrowdCate", "OutDoorID");
            CreateIndex("dbo.OutDoor_AreaCate", "AreaCateID");
            CreateIndex("dbo.OutDoor_AreaCate", "OutDoorID");
            CreateIndex("dbo.OutDoor", "OwnerCode");
            CreateIndex("dbo.OutDoor", "PeriodCode");
            AddForeignKey("dbo.OutDoor_PurposeCate", "PurposeCateID", "dbo.PurposeCate", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OutDoor_PurposeCate", "OutDoorID", "dbo.OutDoor", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OutDoor_IndustryCate", "IndustryCateID", "dbo.IndustryCate", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OutDoor_IndustryCate", "OutDoorID", "dbo.OutDoor", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OutDoor_CrowdCate", "CrowdCateID", "dbo.CrowdCate", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OutDoor_CrowdCate", "OutDoorID", "dbo.OutDoor", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OutDoor_AreaCate", "AreaCateID", "dbo.AreaCate", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OutDoor_AreaCate", "OutDoorID", "dbo.OutDoor", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OutDoor", "OwnerCode", "dbo.OwnerCate", "ID");
            AddForeignKey("dbo.OutDoor", "PeriodCode", "dbo.PeriodCate", "ID");
        }
    }
}
