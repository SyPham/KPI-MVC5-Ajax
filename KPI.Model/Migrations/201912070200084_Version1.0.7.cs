namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version107 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MenuLangs", "Menu_ID", "dbo.Menus");
            DropIndex("dbo.MenuLangs", new[] { "Menu_ID" });
            RenameColumn(table: "dbo.MenuLangs", name: "Menu_ID", newName: "MenuID");
            AlterColumn("dbo.MenuLangs", "MenuID", c => c.Int(nullable: false));
            CreateIndex("dbo.MenuLangs", "MenuID");
            AddForeignKey("dbo.MenuLangs", "MenuID", "dbo.Menus", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuLangs", "MenuID", "dbo.Menus");
            DropIndex("dbo.MenuLangs", new[] { "MenuID" });
            AlterColumn("dbo.MenuLangs", "MenuID", c => c.Int());
            RenameColumn(table: "dbo.MenuLangs", name: "MenuID", newName: "Menu_ID");
            CreateIndex("dbo.MenuLangs", "Menu_ID");
            AddForeignKey("dbo.MenuLangs", "Menu_ID", "dbo.Menus", "ID");
        }
    }
}
