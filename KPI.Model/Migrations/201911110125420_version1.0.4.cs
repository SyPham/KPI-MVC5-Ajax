namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version104 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StateSendMails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ToDay = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StateSendMails");
        }
    }
}
