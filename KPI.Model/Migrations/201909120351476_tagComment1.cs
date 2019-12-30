namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagComment1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "Content");
        }
    }
}
