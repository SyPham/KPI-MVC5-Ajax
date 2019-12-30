namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "TaskName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "TaskName");
        }
    }
}
