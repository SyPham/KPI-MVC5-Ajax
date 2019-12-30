namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version11 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Notifications", name: "TagID", newName: "KPILevelCode");
            AlterColumn("dbo.Notifications", "KPILevelCode", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "KPILevelCode", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Notifications", name: "KPILevelCode", newName: "TagID");
        }
    }
}
