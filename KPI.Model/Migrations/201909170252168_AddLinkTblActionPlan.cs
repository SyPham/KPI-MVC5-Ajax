namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinkTblActionPlan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActionPlans", "Link", c => c.String());
            AddColumn("dbo.Comments", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "Title");
            DropColumn("dbo.ActionPlans", "Link");
        }
    }
}
