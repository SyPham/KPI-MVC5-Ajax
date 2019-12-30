namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version2201 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "IsUpload", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "IsUpload");
        }
    }
}
