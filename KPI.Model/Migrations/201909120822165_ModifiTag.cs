namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiTag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "NotificationID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "NotificationID");
        }
    }
}
