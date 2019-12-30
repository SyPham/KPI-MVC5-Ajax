namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagComment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "Tag", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "Tag", c => c.String());
        }
    }
}
