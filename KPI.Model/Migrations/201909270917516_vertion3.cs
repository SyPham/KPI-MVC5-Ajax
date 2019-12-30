namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vertion3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActionPlans", "Auditor", c => c.Int(nullable: false));
            AddColumn("dbo.KPILevels", "PIC", c => c.Int(nullable: false));
            AddColumn("dbo.KPILevels", "Owner", c => c.Int(nullable: false));
            AddColumn("dbo.KPILevels", "OwnerManagerment", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KPILevels", "OwnerManagerment");
            DropColumn("dbo.KPILevels", "Owner");
            DropColumn("dbo.KPILevels", "PIC");
            DropColumn("dbo.ActionPlans", "Auditor");
        }
    }
}
