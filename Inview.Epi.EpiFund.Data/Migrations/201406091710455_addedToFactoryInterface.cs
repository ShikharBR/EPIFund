namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedToFactoryInterface : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DeferredMaintananceCosts", newName: "DeferredMaintenanceCosts");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DeferredMaintenanceCosts", newName: "DeferredMaintananceCosts");
        }
    }
}
