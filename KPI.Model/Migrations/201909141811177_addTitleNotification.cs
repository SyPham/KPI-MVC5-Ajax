namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTitleNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Title");
        }
    }
}
