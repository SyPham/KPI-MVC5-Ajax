namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version19 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Notifications", name: "Seen", newName: "Action");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Notifications", name: "Action", newName: "Seen");
        }
    }
}
