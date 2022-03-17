namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMaintenanceDetailsToDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeferredMaintananceCosts",
                c => new
                    {
                        DeferredMaintananceCostId = c.Int(nullable: false, identity: true),
                        MaintenanceName = c.Int(nullable: false),
                        Cost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeferredMaintananceCostId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DeferredMaintananceCosts");
        }
    }
}
