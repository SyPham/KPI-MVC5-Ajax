namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActionPlans", "UpdateSheduleDate", c => c.DateTime());
            AlterColumn("dbo.ActionPlans", "ActuralFinishDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ActionPlans", "ActuralFinishDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ActionPlans", "UpdateSheduleDate", c => c.DateTime(nullable: false));
        }
    }
}
