namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTempUnitSpecsAndRenameTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempDeferredMaintenanceItems",
                c => new
                    {
                        TempDeferredMaintenanceItemId = c.Int(nullable: false, identity: true),
                        GuidId = c.Guid(nullable: false),
                        MaintenanceDetail = c.Int(nullable: false),
                        Units = c.Int(nullable: false),
                        UnitCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TempDeferredMaintenanceItemId);
            
            CreateTable(
                "dbo.TempUnitSpecifications",
                c => new
                    {
                        TempUnitSpecificationId = c.Int(nullable: false, identity: true),
                        GuidId = c.Guid(nullable: false),
                        BedCount = c.Int(nullable: false),
                        BathCount = c.Int(nullable: false),
                        UnitBaseRent = c.Single(nullable: false),
                        UnitSquareFeet = c.Int(nullable: false),
                        CountOfUnits = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TempUnitSpecificationId);
            
            DropTable("dbo.TemporaryDeferredMaintenanceItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TemporaryDeferredMaintenanceItems",
                c => new
                    {
                        TemporaryDeferredMaintenanceItemId = c.Int(nullable: false, identity: true),
                        GuidId = c.Guid(nullable: false),
                        MaintenanceDetail = c.Int(nullable: false),
                        Units = c.Int(nullable: false),
                        UnitCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TemporaryDeferredMaintenanceItemId);
            
            DropTable("dbo.TempUnitSpecifications");
            DropTable("dbo.TempDeferredMaintenanceItems");
        }
    }
}
