namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addTargettblData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Data", "Target", c => c.Double(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Data", "Target");
        }
    }
}
