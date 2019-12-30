namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vertion151 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Alias", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Alias");
        }
    }
}
