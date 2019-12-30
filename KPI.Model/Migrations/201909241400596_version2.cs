namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActionPlanDetails", "UserID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ActionPlanDetails", "UserID", c => c.Int(nullable: false));
        }
    }
}
