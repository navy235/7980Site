namespace PadSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SenderID = c.Int(nullable: false),
                        RecipientID = c.Int(nullable: false),
                        Title = c.String(maxLength: 150),
                        Content = c.String(maxLength: 4000),
                        MessageType = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        SenderStatus = c.Int(nullable: false),
                        RecipienterStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Message");
        }
    }
}
