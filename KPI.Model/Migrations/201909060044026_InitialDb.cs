
using System;
using System.Data.Entity.Migrations;
namespace KPI.Model.Migrations
{
    
    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notifications", "Tag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Tag");
            DropColumn("dbo.Notifications", "CreateTime");
        }
    }
}
