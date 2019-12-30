namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActionPlans", "UpdateSheduleDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ActionPlans", "ActuralFinishDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActionPlans", "ActuralFinishDate");
            DropColumn("dbo.ActionPlans", "UpdateSheduleDate");
        }
    }
}
