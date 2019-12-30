namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version71 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Owners", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Owners", "Name", c => c.Int(nullable: false));
        }
    }
}
