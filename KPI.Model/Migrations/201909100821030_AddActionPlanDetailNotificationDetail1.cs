namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActionPlanDetailNotificationDetail1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionPlanDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ActionPlanID = c.Int(nullable: false),
                        USerID = c.Int(nullable: false),
                        Sent = c.Boolean(nullable: false),
                        Seen = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NotificationDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        NotificationID = c.Int(nullable: false),
                        Seen = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NotificationDetails");
            DropTable("dbo.ActionPlanDetails");
        }
    }
}
