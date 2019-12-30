namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vertion17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Managers", "KPILevelCode", c => c.String());
            AddColumn("dbo.Managers", "CategoryCode", c => c.String());
            AddColumn("dbo.Owners", "KPILevelCode", c => c.String());
            AddColumn("dbo.Owners", "CategoryCode", c => c.String());
            AddColumn("dbo.Participants", "KPILevelCode", c => c.String());
            AddColumn("dbo.Participants", "CategoryCode", c => c.String());
            AddColumn("dbo.Sponsors", "KPILevelCode", c => c.String());
            AddColumn("dbo.Sponsors", "CategoryCode", c => c.String());
            AddColumn("dbo.Uploaders", "KPILevelCode", c => c.String());
            AddColumn("dbo.Uploaders", "CategoryCode", c => c.String());
            DropColumn("dbo.Owners", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Owners", "Name", c => c.String());
            DropColumn("dbo.Uploaders", "CategoryCode");
            DropColumn("dbo.Uploaders", "KPILevelCode");
            DropColumn("dbo.Sponsors", "CategoryCode");
            DropColumn("dbo.Sponsors", "KPILevelCode");
            DropColumn("dbo.Participants", "CategoryCode");
            DropColumn("dbo.Participants", "KPILevelCode");
            DropColumn("dbo.Owners", "CategoryCode");
            DropColumn("dbo.Owners", "KPILevelCode");
            DropColumn("dbo.Managers", "CategoryCode");
            DropColumn("dbo.Managers", "KPILevelCode");
        }
    }
}
