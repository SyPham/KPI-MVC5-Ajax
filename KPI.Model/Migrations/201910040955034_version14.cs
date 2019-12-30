namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryKPILevels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        KPILevelID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CategoryKPILevels");
        }
    }
}
