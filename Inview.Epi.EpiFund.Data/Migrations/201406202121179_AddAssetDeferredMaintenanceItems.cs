namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssetDeferredMaintenanceItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetDeferredMaintenanceItems",
                c => new
                    {
                        AssetDeferredMaintenanceItemId = c.Int(nullable: false, identity: true),
                        AssetId = c.Guid(nullable: false),
                        MaintenanceDetail = c.Int(nullable: false),
                        Units = c.Int(nullable: false),
                        UnitCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.AssetDeferredMaintenanceItemId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .Index(t => t.AssetId);
            
            AddColumn("dbo.DeferredMaintenanceCosts", "InputType", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetDeferredMaintenanceItems", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetDeferredMaintenanceItems", new[] { "AssetId" });
            DropColumn("dbo.DeferredMaintenanceCosts", "InputType");
            DropTable("dbo.AssetDeferredMaintenanceItems");
        }
    }
}
