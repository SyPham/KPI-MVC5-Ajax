namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version102 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OCCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OCID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OCCategories");
        }
    }
}
