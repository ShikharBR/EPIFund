namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecurityMeasures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserMachines",
                c => new
                    {
                        UserMachineId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        MachineName = c.String(),
                        Status = c.Int(nullable: false),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.UserMachineId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMachines", "UserId", "dbo.Users");
            DropIndex("dbo.UserMachines", new[] { "UserId" });
            DropTable("dbo.UserMachines");
        }
    }
}
