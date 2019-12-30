namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vertion18 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Data", "Target", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Data", "Target", c => c.Double(nullable: false));
        }
    }
}
