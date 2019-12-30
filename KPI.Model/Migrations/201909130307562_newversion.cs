namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newversion : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tag", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tag", "NotificationID", c => c.Int());
        }
    }
}
