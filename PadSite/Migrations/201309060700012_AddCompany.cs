namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Member_Profile", "CityCode", "dbo.CityCate");
            DropIndex("dbo.Member_Profile", new[] { "CityCode" });
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        MemberID = c.Int(nullable: false),
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        LinkMan = c.String(maxLength: 50),
                        Sex = c.Boolean(nullable: false),
                        Address = c.String(maxLength: 150),
                        Phone = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                        QQ = c.String(maxLength: 50),
                        MSN = c.String(maxLength: 50),
                        Lat = c.Double(nullable: false),
                        Lng = c.Double(nullable: false),
                        CityCode = c.Int(nullable: false),
                        CityCodeValue = c.String(),
                        LastTime = c.DateTime(nullable: false),
                        LastIP = c.String(maxLength: 50),
                        AddTime = c.DateTime(nullable: false),
                        AddIP = c.String(maxLength: 50),
                        Description = c.String(maxLength: 2000),
                        Unapprovedlog = c.String(maxLength: 500),
                        LinkManImg = c.String(maxLength: 2000),
                        CredentialsImg = c.String(maxLength: 2000),
                        LogoImg = c.String(maxLength: 200),
                        BannerImg = c.String(maxLength: 200),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MemberID)
                .ForeignKey("dbo.Member", t => t.MemberID)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.CompanyCredentialsImg",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberID = c.Int(nullable: false),
                        ImgUrl = c.String(maxLength: 200),
                        Title = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.CompanyMessage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Content = c.String(maxLength: 2000),
                        Title = c.String(maxLength: 50),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.CompanyNotice",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberID = c.Int(nullable: false),
                        Content = c.String(maxLength: 2000),
                        Title = c.String(maxLength: 50),
                        AddTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompanyNotice", new[] { "MemberID" });
            DropIndex("dbo.CompanyMessage", new[] { "MemberID" });
            DropIndex("dbo.CompanyCredentialsImg", new[] { "MemberID" });
            DropIndex("dbo.Company", new[] { "MemberID" });
            DropForeignKey("dbo.CompanyNotice", "MemberID", "dbo.Member");
            DropForeignKey("dbo.CompanyMessage", "MemberID", "dbo.Member");
            DropForeignKey("dbo.CompanyCredentialsImg", "MemberID", "dbo.Member");
            DropForeignKey("dbo.Company", "MemberID", "dbo.Member");
            DropTable("dbo.CompanyNotice");
            DropTable("dbo.CompanyMessage");
            DropTable("dbo.CompanyCredentialsImg");
            DropTable("dbo.Company");
            CreateIndex("dbo.Member_Profile", "CityCode");
            AddForeignKey("dbo.Member_Profile", "CityCode", "dbo.CityCate", "ID", cascadeDelete: true);
        }
    }
}
