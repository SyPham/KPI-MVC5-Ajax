namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyValueTblData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Data", "Value", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Data", "Value", c => c.Int(nullable: false));
        }
    }
}
