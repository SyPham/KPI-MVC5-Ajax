namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version101 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menus", "Position");
        }
    }
}
