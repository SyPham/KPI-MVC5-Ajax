namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagComment2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Comments", "KPILevelCode");
            DropColumn("dbo.Comments", "Tag");
            DropColumn("dbo.Comments", "Period");
            DropColumn("dbo.Tags", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "Content", c => c.String());
            AddColumn("dbo.Comments", "Period", c => c.String());
            AddColumn("dbo.Comments", "Tag", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "KPILevelCode", c => c.String());
        }
    }
}
