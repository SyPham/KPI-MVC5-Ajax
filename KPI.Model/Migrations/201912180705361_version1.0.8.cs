namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version108 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubNotifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        KPIName = c.String(),
                        URL = c.String(),
                        UserID = c.Int(nullable: false),
                        NotificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Notifications", t => t.NotificationID, cascadeDelete: true)
                .Index(t => t.NotificationID);
            
            AddColumn("dbo.NotificationDetails", "Content", c => c.String());
            AddColumn("dbo.NotificationDetails", "URL", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubNotifications", "NotificationID", "dbo.Notifications");
            DropIndex("dbo.SubNotifications", new[] { "NotificationID" });
            DropColumn("dbo.NotificationDetails", "URL");
            DropColumn("dbo.NotificationDetails", "Content");
            DropTable("dbo.SubNotifications");
        }
    }
}
