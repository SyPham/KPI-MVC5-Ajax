namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version103 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActionPlans", "KPILevelCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActionPlans", "KPILevelCode");
        }
    }
}
