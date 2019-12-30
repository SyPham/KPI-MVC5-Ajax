namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTagTblComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Tag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "Tag");
        }
    }
}
