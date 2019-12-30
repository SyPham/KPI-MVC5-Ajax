namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version109 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LateOnUpLoads",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        NotificationID = c.Int(nullable: false),
                        Area = c.String(),
                        DeadLine = c.String(),
                        KPIName = c.String(),
                        Code = c.String(),
                        Year = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LateOnUpLoads");
        }
    }
}
