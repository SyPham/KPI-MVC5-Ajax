namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version9 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ActionPlans", name: "ActuralFinishDate", newName: "ActualFinishDate");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.ActionPlans", name: "ActualFinishDate", newName: "ActuralFinishDate");
        }
    }
}
