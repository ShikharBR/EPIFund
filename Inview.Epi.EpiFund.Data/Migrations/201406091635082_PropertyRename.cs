namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertyRename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeferredMaintananceCosts", "MaintenanceDetail", c => c.Int(nullable: false));
            DropColumn("dbo.DeferredMaintananceCosts", "MaintenanceName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeferredMaintananceCosts", "MaintenanceName", c => c.Int(nullable: false));
            DropColumn("dbo.DeferredMaintananceCosts", "MaintenanceDetail");
        }
    }
}
