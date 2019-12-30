namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vertion13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotificationDetails", "Action", c => c.String());
            DropColumn("dbo.Uploader", "KPILevelID");
            DropColumn("dbo.Manager", "KPILevelID");
            DropColumn("dbo.Owner", "KPILevelID");

        }

        public override void Down()
        {
            DropColumn("dbo.NotificationDetails", "Action");
        }
    }
}
