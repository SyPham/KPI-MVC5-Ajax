namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vertion12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        KPILevelID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Uploaders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        KPILevelID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Owners", "KPILevelID", c => c.Int(nullable: false));
            AddColumn("dbo.Owners", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.Owners", "CreatedTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Owners", "CreatedTime");
            DropColumn("dbo.Owners", "UserID");
            DropColumn("dbo.Owners", "KPILevelID");
            DropTable("dbo.Uploaders");
            DropTable("dbo.Managers");
        }
    }
}
