namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ErrorMessages", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.ErrorMessages", "Function", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ErrorMessages", "Function");
            DropColumn("dbo.ErrorMessages", "CreateTime");
        }
    }
}
