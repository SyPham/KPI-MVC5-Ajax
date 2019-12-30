namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version8 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Users", name: "Ower", newName: "Owner");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Users", name: "Owner", newName: "Ower");
        }
    }
}
