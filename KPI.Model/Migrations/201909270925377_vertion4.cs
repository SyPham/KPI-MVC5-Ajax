namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vertion4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Data", "Yearly", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Data", "Yearly");
        }
    }
}
