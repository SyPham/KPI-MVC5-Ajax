namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Managers", "CategoryID", c => c.Int(nullable: false));
            AddColumn("dbo.Owners", "CategoryID", c => c.Int(nullable: false));
            AddColumn("dbo.Participants", "CategoryID", c => c.Int(nullable: false));
            AddColumn("dbo.Sponsors", "CategoryID", c => c.Int(nullable: false));
            AddColumn("dbo.Uploaders", "CategoryID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Uploaders", "CategoryID");
            DropColumn("dbo.Sponsors", "CategoryID");
            DropColumn("dbo.Participants", "CategoryID");
            DropColumn("dbo.Owners", "CategoryID");
            DropColumn("dbo.Managers", "CategoryID");
        }
    }
}
