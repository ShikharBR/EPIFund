namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetDocuments",
                c => new
                    {
                        AssetDocumentId = c.Guid(nullable: false),
                        Location = c.String(),
                        Title = c.String(),
                        Size = c.String(),
                        Asset_AssetId = c.Guid(),
                    })
                .PrimaryKey(t => t.AssetDocumentId)
                .ForeignKey("dbo.Assets", t => t.Asset_AssetId)
                .Index(t => t.Asset_AssetId);
            
            CreateTable(
                "dbo.AssetHoas",
                c => new
                    {
                        AssetHoaId = c.Guid(nullable: false),
                        AnnualFee = c.Int(nullable: false),
                        Fee = c.Int(nullable: false),
                        Frequency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssetHoaId);
            
            CreateTable(
                "dbo.AssetImages",
                c => new
                    {
                        AssetImageId = c.Guid(nullable: false),
                        Location = c.String(),
                        Asset_AssetId = c.Guid(),
                    })
                .PrimaryKey(t => t.AssetImageId)
                .ForeignKey("dbo.Assets", t => t.Asset_AssetId)
                .Index(t => t.Asset_AssetId);
            
            CreateTable(
                "dbo.AssetIncomes",
                c => new
                    {
                        AssetIncomeId = c.Guid(nullable: false),
                        MonthlyGrossIncome = c.Int(nullable: false),
                        AnnualGrossIncome = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssetIncomeId);
            
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        AssetId = c.Guid(nullable: false),
                        AssetCategory = c.Int(nullable: false),
                        AssetType = c.Int(nullable: false),
                        OperatingStatus = c.String(),
                        Owner = c.String(),
                        ContactPhoneNumber = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        County = c.String(),
                        LotNumber = c.String(),
                        Subdivision = c.String(),
                        TaxBookMap = c.String(),
                        TaxParcelNumber = c.String(),
                        PropertyCondition = c.Int(nullable: false),
                        OccupancyType = c.Int(nullable: false),
                        YearBuilt = c.Int(nullable: false),
                        SquareFeet = c.Int(nullable: false),
                        LotSize = c.String(),
                        ParkingSpaces = c.Int(nullable: false),
                        ListedByRealtor = c.Boolean(nullable: false),
                        MlsId = c.Int(nullable: false),
                        LocalMlsDefinition = c.String(),
                        AnnualPropertyTax = c.Int(nullable: false),
                        PropertyTaxYear = c.Int(nullable: false),
                        CurrentPropertyTaxes = c.Boolean(nullable: false),
                        YearsInArrearTaxes = c.Int(nullable: false),
                        OriginalAppraisal = c.Boolean(nullable: false),
                        CurrentBpo = c.Int(nullable: false),
                        AskingPrice = c.Int(nullable: false),
                        NonRetailPricingComparativePercentage = c.Single(nullable: false),
                        EquityMargin = c.Single(nullable: false),
                        CashInvestmentApy = c.Single(nullable: false),
                        ClosingDate = c.DateTime(nullable: false),
                        ProposedBuyer = c.String(),
                        ProposedBuyerAddress = c.String(),
                        ProposedBuyerContact = c.String(),
                        ProposedBuyerEntityType = c.String(),
                        Deposit = c.Single(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Show = c.Boolean(nullable: false),
                        Sold = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        GeneralComments = c.String(),
                        Comments = c.String(),
                        RetailComments = c.String(),
                        OccupancyPercentage = c.Single(),
                        EstDeferredMaintenance = c.Int(),
                        MeterMethod = c.Int(),
                        TotalSqareFootage = c.Int(),
                        RentableSquareFeet = c.Int(),
                        NumberOfTenants = c.Int(),
                        Type = c.Int(),
                        OccupancyPercentage1 = c.Single(),
                        EstDeferredMaintenance1 = c.Int(),
                        MeterMethod1 = c.Int(),
                        TotalSqareFootage1 = c.Int(),
                        TotalUnits = c.Int(),
                        GrossRentableSquareFeet = c.Int(),
                        ListPrice = c.Int(),
                        ListPricePerUnit = c.Single(),
                        ListPricePerSquareFoot = c.Single(),
                        ProformaSgi = c.Int(),
                        proformaOperExp = c.Int(),
                        SgiPercet = c.Single(),
                        ProformaMiscIncome = c.Int(),
                        ProformaPreTaxNoi = c.Int(),
                        ListedPriceCapRate = c.Single(),
                        AgentName = c.String(),
                        AgentPhone = c.String(),
                        RetailAskingPrice = c.Int(),
                        RetailMargin = c.Int(),
                        SellerContribuition = c.Int(),
                        SellerContributionPercentage = c.Single(),
                        ComparativePercentage = c.Single(),
                        HazardInsurance = c.Int(),
                        PropertyType = c.Int(),
                        Features = c.String(),
                        BathCount = c.Int(),
                        BedCount = c.Int(),
                        PropertyDetails = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Foreclosure_ForeclosureInfoId = c.Guid(),
                        HOA_AssetHoaId = c.Guid(),
                        Income_AssetIncomeId = c.Guid(),
                        MainImage_AssetImageId = c.Guid(),
                        Paper_PaperAssetId = c.Guid(),
                        EstDefMaintenance_EstimatedDefMaintenanceId = c.Guid(),
                        LastReportedOccupancy_ReportedOccupancyId = c.Guid(),
                    })
                .PrimaryKey(t => t.AssetId)
                .ForeignKey("dbo.ForeclosureInfoes", t => t.Foreclosure_ForeclosureInfoId)
                .ForeignKey("dbo.AssetHoas", t => t.HOA_AssetHoaId)
                .ForeignKey("dbo.AssetIncomes", t => t.Income_AssetIncomeId)
                .ForeignKey("dbo.AssetImages", t => t.MainImage_AssetImageId)
                .ForeignKey("dbo.PaperAssets", t => t.Paper_PaperAssetId)
                .ForeignKey("dbo.EstimatedDefMaintenances", t => t.EstDefMaintenance_EstimatedDefMaintenanceId)
                .ForeignKey("dbo.ReportedOccupancies", t => t.LastReportedOccupancy_ReportedOccupancyId)
                .Index(t => t.Foreclosure_ForeclosureInfoId)
                .Index(t => t.HOA_AssetHoaId)
                .Index(t => t.Income_AssetIncomeId)
                .Index(t => t.MainImage_AssetImageId)
                .Index(t => t.Paper_PaperAssetId)
                .Index(t => t.EstDefMaintenance_EstimatedDefMaintenanceId)
                .Index(t => t.LastReportedOccupancy_ReportedOccupancyId);
            
            CreateTable(
                "dbo.ForeclosureInfoes",
                c => new
                    {
                        ForeclosureInfoId = c.Guid(nullable: false),
                        Lender = c.String(),
                        Position = c.Int(nullable: false),
                        RecordNumber = c.Int(nullable: false),
                        OriginalMortgageAmount = c.Int(nullable: false),
                        OriginalMortageDate = c.DateTime(nullable: false),
                        SaleDate = c.DateTime(nullable: false),
                        RecordDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ForeclosureInfoId);
            
            CreateTable(
                "dbo.PaperAssets",
                c => new
                    {
                        PaperAssetId = c.Guid(nullable: false),
                        PaperType = c.Int(nullable: false),
                        PropertyType = c.Int(nullable: false),
                        ServicingAgent = c.String(),
                        Assignor = c.String(),
                        PrincipalBalance = c.Int(nullable: false),
                        AskingPrice = c.Int(nullable: false),
                        ApyForAskingPrice = c.Single(nullable: false),
                        MonthlyInterestIncome = c.Int(nullable: false),
                        EquityMargin = c.Int(nullable: false),
                        MonthsInArrears = c.Int(nullable: false),
                        MaturityDate = c.DateTime(nullable: false),
                        NextDueDate = c.DateTime(nullable: false),
                        OriginationDate = c.DateTime(nullable: false),
                        PriorityMortgageBalance = c.Int(nullable: false),
                        OriginalInstrDocument = c.String(),
                        Current = c.Boolean(nullable: false),
                        NoteRate = c.Single(nullable: false),
                        InvestmentYield = c.Single(nullable: false),
                        Priority = c.Int(nullable: false),
                        Ltv = c.Single(nullable: false),
                        Cltv = c.Single(nullable: false),
                        Successor = c.String(),
                        SuccessorAddress = c.String(),
                        SuccessorRecordedDocNumber = c.String(),
                        SuccessorDocDate = c.String(),
                        SuccessorType = c.String(),
                        Trustor = c.String(),
                        Trustee = c.String(),
                    })
                .PrimaryKey(t => t.PaperAssetId);
            
            CreateTable(
                "dbo.EstimatedDefMaintenances",
                c => new
                    {
                        EstimatedDefMaintenanceId = c.Guid(nullable: false),
                        EstimatedCost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EstimatedDefMaintenanceId);
            
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
                "dbo.AssetUnitSpecifications",
                c => new
                    {
                        AssetUnitSpecificationId = c.Guid(nullable: false),
                        BedCount = c.Int(nullable: false),
                        BathCount = c.Int(nullable: false),
                        UnitBaseRent = c.Single(nullable: false),
                        UnitSquareFeet = c.Int(nullable: false),
                        CountOfUnits = c.Int(nullable: false),
                        MultiFamilyAsset_AssetId = c.Guid(),
                    })
                .PrimaryKey(t => t.AssetUnitSpecificationId)
                .ForeignKey("dbo.Assets", t => t.MultiFamilyAsset_AssetId)
                .Index(t => t.MultiFamilyAsset_AssetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetUnitSpecifications", "MultiFamilyAsset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.Assets", "LastReportedOccupancy_ReportedOccupancyId", "dbo.ReportedOccupancies");
            DropForeignKey("dbo.Assets", "EstDefMaintenance_EstimatedDefMaintenanceId", "dbo.EstimatedDefMaintenances");
            DropForeignKey("dbo.Assets", "Paper_PaperAssetId", "dbo.PaperAssets");
            DropForeignKey("dbo.Assets", "MainImage_AssetImageId", "dbo.AssetImages");
            DropForeignKey("dbo.Assets", "Income_AssetIncomeId", "dbo.AssetIncomes");
            DropForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.Assets", "HOA_AssetHoaId", "dbo.AssetHoas");
            DropForeignKey("dbo.Assets", "Foreclosure_ForeclosureInfoId", "dbo.ForeclosureInfoes");
            DropForeignKey("dbo.AssetDocuments", "Asset_AssetId", "dbo.Assets");
            DropIndex("dbo.AssetUnitSpecifications", new[] { "MultiFamilyAsset_AssetId" });
            DropIndex("dbo.Assets", new[] { "LastReportedOccupancy_ReportedOccupancyId" });
            DropIndex("dbo.Assets", new[] { "EstDefMaintenance_EstimatedDefMaintenanceId" });
            DropIndex("dbo.Assets", new[] { "Paper_PaperAssetId" });
            DropIndex("dbo.Assets", new[] { "MainImage_AssetImageId" });
            DropIndex("dbo.Assets", new[] { "Income_AssetIncomeId" });
            DropIndex("dbo.Assets", new[] { "HOA_AssetHoaId" });
            DropIndex("dbo.Assets", new[] { "Foreclosure_ForeclosureInfoId" });
            DropIndex("dbo.AssetImages", new[] { "Asset_AssetId" });
            DropIndex("dbo.AssetDocuments", new[] { "Asset_AssetId" });
            DropTable("dbo.AssetUnitSpecifications");
            DropTable("dbo.ReportedOccupancies");
            DropTable("dbo.EstimatedDefMaintenances");
            DropTable("dbo.PaperAssets");
            DropTable("dbo.ForeclosureInfoes");
            DropTable("dbo.Assets");
            DropTable("dbo.AssetIncomes");
            DropTable("dbo.AssetImages");
            DropTable("dbo.AssetHoas");
            DropTable("dbo.AssetDocuments");
        }
    }
}
