namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOutDoor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OutDoor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Description = c.String(maxLength: 2000),
                        Integrity = c.Int(nullable: false),
                        Hit = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        LastTime = c.DateTime(nullable: false),
                        Favorite = c.Int(nullable: false),
                        Message = c.Int(nullable: false),
                        MemberID = c.Int(nullable: false),
                        AddIP = c.String(maxLength: 50),
                        AdminUser = c.Int(nullable: false),
                        LastIP = c.String(maxLength: 50),
                        Unapprovedlog = c.String(maxLength: 500),
                        SeoTitle = c.String(maxLength: 100),
                        SeoDes = c.String(maxLength: 250),
                        Seokeywords = c.String(maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceExten = c.String(maxLength: 100),
                        Location = c.String(maxLength: 100),
                        Lng = c.Double(nullable: false),
                        Lat = c.Double(nullable: false),
                        HasLight = c.Boolean(nullable: false),
                        LightStrat = c.String(maxLength: 50),
                        LightEnd = c.String(maxLength: 50),
                        VideoUrl = c.String(maxLength: 250),
                        Wdith = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Height = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalFaces = c.Int(nullable: false),
                        TrafficAuto = c.Int(nullable: false),
                        TrafficPerson = c.Int(nullable: false),
                        CityCode = c.Int(nullable: false),
                        CityCodeValue = c.String(),
                        FormatCode = c.Int(nullable: false),
                        MeidaCode = c.Int(nullable: false),
                        PeriodCode = c.Int(nullable: false),
                        OwnerCode = c.Int(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        AuthStatus = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        MediaImg = c.String(),
                        MediaFoucsImg = c.String(),
                        CredentialsImg = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CityCate", t => t.CityCode)
                .ForeignKey("dbo.MediaCate", t => t.MeidaCode)
                .ForeignKey("dbo.PeriodCate", t => t.PeriodCode)
                .ForeignKey("dbo.FormatCate", t => t.FormatCode)
                .ForeignKey("dbo.OwnerCate", t => t.OwnerCode)
                .ForeignKey("dbo.Member", t => t.MemberID)
                .Index(t => t.CityCode)
                .Index(t => t.MeidaCode)
                .Index(t => t.PeriodCode)
                .Index(t => t.FormatCode)
                .Index(t => t.OwnerCode)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.AreaCate",
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
                "dbo.IndustryCate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CateName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OwnerCate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CateName = c.String(maxLength: 50),
                        PID = c.Int(),
                        Code = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        OrderIndex = c.Int(nullable: false),
                        PCate_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OwnerCate", t => t.PCate_ID)
                .Index(t => t.PCate_ID);
            
            CreateTable(
                "dbo.OutDoor_AreaCate",
                c => new
                    {
                        OutDoorID = c.Int(nullable: false),
                        AreaCateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OutDoorID, t.AreaCateID })
                .ForeignKey("dbo.OutDoor", t => t.OutDoorID, cascadeDelete: true)
                .ForeignKey("dbo.AreaCate", t => t.AreaCateID, cascadeDelete: true)
                .Index(t => t.OutDoorID)
                .Index(t => t.AreaCateID);
            
            CreateTable(
                "dbo.OutDoor_CrowdCate",
                c => new
                    {
                        OutDoorID = c.Int(nullable: false),
                        CrowdCateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OutDoorID, t.CrowdCateID })
                .ForeignKey("dbo.OutDoor", t => t.OutDoorID, cascadeDelete: true)
                .ForeignKey("dbo.CrowdCate", t => t.CrowdCateID, cascadeDelete: true)
                .Index(t => t.OutDoorID)
                .Index(t => t.CrowdCateID);
            
            CreateTable(
                "dbo.OutDoor_IndustryCate",
                c => new
                    {
                        OutDoorID = c.Int(nullable: false),
                        IndustryCateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OutDoorID, t.IndustryCateID })
                .ForeignKey("dbo.OutDoor", t => t.OutDoorID, cascadeDelete: true)
                .ForeignKey("dbo.IndustryCate", t => t.IndustryCateID, cascadeDelete: true)
                .Index(t => t.OutDoorID)
                .Index(t => t.IndustryCateID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.OutDoor_IndustryCate", new[] { "IndustryCateID" });
            DropIndex("dbo.OutDoor_IndustryCate", new[] { "OutDoorID" });
            DropIndex("dbo.OutDoor_CrowdCate", new[] { "CrowdCateID" });
            DropIndex("dbo.OutDoor_CrowdCate", new[] { "OutDoorID" });
            DropIndex("dbo.OutDoor_AreaCate", new[] { "AreaCateID" });
            DropIndex("dbo.OutDoor_AreaCate", new[] { "OutDoorID" });
            DropIndex("dbo.OwnerCate", new[] { "PCate_ID" });
            DropIndex("dbo.OutDoor", new[] { "MemberID" });
            DropIndex("dbo.OutDoor", new[] { "OwnerCode" });
            DropIndex("dbo.OutDoor", new[] { "FormatCode" });
            DropIndex("dbo.OutDoor", new[] { "PeriodCode" });
            DropIndex("dbo.OutDoor", new[] { "MeidaCode" });
            DropIndex("dbo.OutDoor", new[] { "CityCode" });
            DropForeignKey("dbo.OutDoor_IndustryCate", "IndustryCateID", "dbo.IndustryCate");
            DropForeignKey("dbo.OutDoor_IndustryCate", "OutDoorID", "dbo.OutDoor");
            DropForeignKey("dbo.OutDoor_CrowdCate", "CrowdCateID", "dbo.CrowdCate");
            DropForeignKey("dbo.OutDoor_CrowdCate", "OutDoorID", "dbo.OutDoor");
            DropForeignKey("dbo.OutDoor_AreaCate", "AreaCateID", "dbo.AreaCate");
            DropForeignKey("dbo.OutDoor_AreaCate", "OutDoorID", "dbo.OutDoor");
            DropForeignKey("dbo.OwnerCate", "PCate_ID", "dbo.OwnerCate");
            DropForeignKey("dbo.OutDoor", "MemberID", "dbo.Member");
            DropForeignKey("dbo.OutDoor", "OwnerCode", "dbo.OwnerCate");
            DropForeignKey("dbo.OutDoor", "FormatCode", "dbo.FormatCate");
            DropForeignKey("dbo.OutDoor", "PeriodCode", "dbo.PeriodCate");
            DropForeignKey("dbo.OutDoor", "MeidaCode", "dbo.MediaCate");
            DropForeignKey("dbo.OutDoor", "CityCode", "dbo.CityCate");
            DropTable("dbo.OutDoor_IndustryCate");
            DropTable("dbo.OutDoor_CrowdCate");
            DropTable("dbo.OutDoor_AreaCate");
            DropTable("dbo.OwnerCate");
            DropTable("dbo.IndustryCate");
            DropTable("dbo.CrowdCate");
            DropTable("dbo.AreaCate");
            DropTable("dbo.OutDoor");
        }
    }
}
