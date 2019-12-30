namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version106 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuLangs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LangID = c.String(),
                        Name = c.String(),
                        CreatedTime = c.DateTime(nullable: false),
                        Menu_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Menus", t => t.Menu_ID)
                .Index(t => t.Menu_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuLangs", "Menu_ID", "dbo.Menus");
            DropIndex("dbo.MenuLangs", new[] { "Menu_ID" });
            DropTable("dbo.MenuLangs");
        }
    }
}
