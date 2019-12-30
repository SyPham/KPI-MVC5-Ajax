namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notifications", "Action", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "Action", c => c.Boolean(nullable: false));
        }
    }
}
