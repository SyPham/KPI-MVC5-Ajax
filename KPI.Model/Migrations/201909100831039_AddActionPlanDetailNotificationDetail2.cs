namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActionPlanDetailNotificationDetail2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        CommentID = c.Int(nullable: false),
                        ActionPlanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ActionPlans", "TagID", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "TagID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "TagID");
            DropColumn("dbo.ActionPlans", "TagID");
            DropTable("dbo.Tags");
        }
    }
}
