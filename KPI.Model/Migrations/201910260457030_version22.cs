namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoryKPILevels", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CategoryKPILevels", "Status");
        }
    }
}
