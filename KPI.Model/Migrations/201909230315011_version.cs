namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "CommentID", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "ActionplanID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "ActionplanID");
            DropColumn("dbo.Notifications", "CommentID");
        }
    }
}
