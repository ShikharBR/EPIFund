namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingCommercialAsset : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "EstDefMaintenance_EstimatedDefMaintenanceId", "dbo.EstimatedDefMaintenances");
            DropForeignKey("dbo.Assets", "LastReportedOccupancy_ReportedOccupancyId", "dbo.ReportedOccupancies");
            DropIndex("dbo.Assets", new[] { "EstDefMaintenance_EstimatedDefMaintenanceId" });
            DropIndex("dbo.Assets", new[] { "LastReportedOccupancy_ReportedOccupancyId" });
            DropColumn("dbo.Assets", "EstDeferredMaintenance");
            RenameColumn(table: "dbo.Assets", name: "EstDeferredMaintenance1", newName: "EstDeferredMaintenance");
            AddColumn("dbo.Assets", "ProformaAnnualNoi", c => c.Int());
            AddColumn("dbo.Assets", "HasAAARatedMajorTenant", c => c.Boolean());
            AddColumn("dbo.Assets", "NameOfAAARatedMajorTenant", c => c.String());
            AddColumn("dbo.Assets", "NumberofSuites", c => c.Int());
            AddColumn("dbo.Assets", "OccupancyDate", c => c.DateTime());
            AddColumn("dbo.Assets", "VacantSuites", c => c.Int());
            AddColumn("dbo.Assets", "LastReportedDate", c => c.DateTime());
            AddColumn("dbo.Assets", "ElectricMeterMethod", c => c.Int());
            AddColumn("dbo.Assets", "GasMeterMethod", c => c.Int());
            AddColumn("dbo.Assets", "ParkOwnedMHUnits", c => c.Int());
            AddColumn("dbo.Assets", "HasDeferredMaintenance", c => c.Boolean());
            AlterColumn("dbo.Assets", "ClosingDate", c => c.DateTime());
            AlterColumn("dbo.Assets", "ActualClosingDate", c => c.DateTime());
            AlterColumn("dbo.Assets", "DateOfCsaConfirm", c => c.DateTime());
            AlterColumn("dbo.Assets", "DateCommissionToEpiReceived", c => c.DateTime());
            DropColumn("dbo.Assets", "OriginalAppraisal");
            DropColumn("dbo.Assets", "MeterMethod");
            DropColumn("dbo.Assets", "TotalSqareFootage");
            DropColumn("dbo.Assets", "MeterMethod1");
            DropColumn("dbo.Assets", "ListPrice");
            DropColumn("dbo.Assets", "ListPricePerUnit");
            DropColumn("dbo.Assets", "ListPricePerSquareFoot");
            DropColumn("dbo.Assets", "ProformaOperExp");
            DropColumn("dbo.Assets", "SgiPercent");
            DropColumn("dbo.Assets", "ProformaMiscIncome");
            DropColumn("dbo.Assets", "ProformaPreTaxNoi");
            DropColumn("dbo.Assets", "ListedPriceCapRate");
            DropColumn("dbo.Assets", "AgentName");
            DropColumn("dbo.Assets", "AgentPhone");
            DropColumn("dbo.Assets", "RetailAskingPrice");
            DropColumn("dbo.Assets", "RetailMargin");
            DropColumn("dbo.Assets", "SellerContribuition");
            DropColumn("dbo.Assets", "SellerContributionPercentage");
            DropColumn("dbo.Assets", "ComparativePercentage");
            DropColumn("dbo.Assets", "HazardInsurance");
            DropColumn("dbo.Assets", "PropertyType");
            DropColumn("dbo.Assets", "Features");
            DropColumn("dbo.Assets", "PropertyDetails");
            DropColumn("dbo.Assets", "EstDefMaintenance_EstimatedDefMaintenanceId");
            DropColumn("dbo.Assets", "LastReportedOccupancy_ReportedOccupancyId");
            DropTable("dbo.EstimatedDefMaintenances");
            DropTable("dbo.ReportedOccupancies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ReportedOccupancies",
                c => new
                    {
                        ReportedOccupancyId = c.Guid(nullable: false),
                        LastReportedDate = c.DateTime(nullable: false),
                        Percentage = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ReportedOccupancyId);
            
            CreateTable(
                "dbo.EstimatedDefMaintenances",
                c => new
                    {
                        EstimatedDefMaintenanceId = c.Guid(nullable: false),
                        EstimatedCost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EstimatedDefMaintenanceId);
            
            AddColumn("dbo.Assets", "LastReportedOccupancy_ReportedOccupancyId", c => c.Guid());
            AddColumn("dbo.Assets", "EstDefMaintenance_EstimatedDefMaintenanceId", c => c.Guid());
            AddColumn("dbo.Assets", "PropertyDetails", c => c.Int());
            AddColumn("dbo.Assets", "Features", c => c.String());
            AddColumn("dbo.Assets", "PropertyType", c => c.Int());
            AddColumn("dbo.Assets", "HazardInsurance", c => c.Int());
            AddColumn("dbo.Assets", "ComparativePercentage", c => c.Single());
            AddColumn("dbo.Assets", "SellerContributionPercentage", c => c.Single());
            AddColumn("dbo.Assets", "SellerContribuition", c => c.Int());
            AddColumn("dbo.Assets", "RetailMargin", c => c.Int());
            AddColumn("dbo.Assets", "RetailAskingPrice", c => c.Int());
            AddColumn("dbo.Assets", "AgentPhone", c => c.String());
            AddColumn("dbo.Assets", "AgentName", c => c.String());
            AddColumn("dbo.Assets", "ListedPriceCapRate", c => c.Single());
            AddColumn("dbo.Assets", "ProformaPreTaxNoi", c => c.Int());
            AddColumn("dbo.Assets", "ProformaMiscIncome", c => c.Int());
            AddColumn("dbo.Assets", "SgiPercent", c => c.Single());
            AddColumn("dbo.Assets", "ProformaOperExp", c => c.Int());
            AddColumn("dbo.Assets", "ListPricePerSquareFoot", c => c.Single());
            AddColumn("dbo.Assets", "ListPricePerUnit", c => c.Single());
            AddColumn("dbo.Assets", "ListPrice", c => c.Int());
            AddColumn("dbo.Assets", "MeterMethod1", c => c.Int());
            AddColumn("dbo.Assets", "TotalSqareFootage", c => c.Int());
            AddColumn("dbo.Assets", "MeterMethod", c => c.Int());
            AddColumn("dbo.Assets", "OriginalAppraisal", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Assets", "DateCommissionToEpiReceived", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Assets", "DateOfCsaConfirm", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Assets", "ActualClosingDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Assets", "ClosingDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Assets", "HasDeferredMaintenance");
            DropColumn("dbo.Assets", "ParkOwnedMHUnits");
            DropColumn("dbo.Assets", "GasMeterMethod");
            DropColumn("dbo.Assets", "ElectricMeterMethod");
            DropColumn("dbo.Assets", "LastReportedDate");
            DropColumn("dbo.Assets", "VacantSuites");
            DropColumn("dbo.Assets", "OccupancyDate");
            DropColumn("dbo.Assets", "NumberofSuites");
            DropColumn("dbo.Assets", "NameOfAAARatedMajorTenant");
            DropColumn("dbo.Assets", "HasAAARatedMajorTenant");
            DropColumn("dbo.Assets", "ProformaAnnualNoi");
            RenameColumn(table: "dbo.Assets", name: "EstDeferredMaintenance", newName: "EstDeferredMaintenance1");
            AddColumn("dbo.Assets", "EstDeferredMaintenance", c => c.Int());
            CreateIndex("dbo.Assets", "LastReportedOccupancy_ReportedOccupancyId");
            CreateIndex("dbo.Assets", "EstDefMaintenance_EstimatedDefMaintenanceId");
            AddForeignKey("dbo.Assets", "LastReportedOccupancy_ReportedOccupancyId", "dbo.ReportedOccupancies", "ReportedOccupancyId");
            AddForeignKey("dbo.Assets", "EstDefMaintenance_EstimatedDefMaintenanceId", "dbo.EstimatedDefMaintenances", "EstimatedDefMaintenanceId");
        }
    }
}
