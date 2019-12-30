namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNotification : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Notifications", name: "DataID", newName: "KPIName");
            RenameColumn(table: "dbo.Notifications", name: "CommentID", newName: "Period");
            RenameColumn(table: "dbo.Notifications", name: "Content", newName: "Link");
            AlterColumn("dbo.Notifications", "KPIName", c => c.String());
            AlterColumn("dbo.Notifications", "Period", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "Period", c => c.Int(nullable: false));
            AlterColumn("dbo.Notifications", "KPIName", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Notifications", name: "Link", newName: "Content");
            RenameColumn(table: "dbo.Notifications", name: "Period", newName: "CommentID");
            RenameColumn(table: "dbo.Notifications", name: "KPIName", newName: "DataID");
        }
    }
}
