namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reworkingAssets : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "HOA_AssetHoaId", "dbo.AssetHoas");
            DropIndex("dbo.Assets", new[] { "HOA_AssetHoaId" });
            AddColumn("dbo.Assets", "PropertyAddress", c => c.String());
            AddColumn("dbo.Assets", "ActualClosingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Assets", "DateOfCsaConfirm", c => c.DateTime(nullable: false));
            AddColumn("dbo.Assets", "CommissionShareAgr", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "CommissionShareToEPI", c => c.Double(nullable: false));
            AddColumn("dbo.Assets", "DateCommissionToEpiReceived", c => c.DateTime(nullable: false));
            AddColumn("dbo.Assets", "ListingAgentCompany", c => c.String());
            DropColumn("dbo.Assets", "Address");
            DropColumn("dbo.Assets", "HasHOA");
            DropColumn("dbo.Assets", "ListedByRealtor");
            DropColumn("dbo.Assets", "MlsId");
            DropColumn("dbo.Assets", "LocalMlsDefinition");
            DropColumn("dbo.Assets", "NonRetailPricingComparativePercentage");
            DropColumn("dbo.Assets", "EquityMargin");
            DropColumn("dbo.Assets", "ProposedBuyerEntityType");
            DropColumn("dbo.Assets", "Deposit");
            DropColumn("dbo.Assets", "CommissionOwed");
            DropColumn("dbo.Assets", "DaysBeforeListing");
            DropColumn("dbo.Assets", "HOA_AssetHoaId");
            DropTable("dbo.AssetHoas");
        }
        
        public override void Down()
        {
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
            
            AddColumn("dbo.Assets", "HOA_AssetHoaId", c => c.Guid());
            AddColumn("dbo.Assets", "DaysBeforeListing", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "CommissionOwed", c => c.Double(nullable: false));
            AddColumn("dbo.Assets", "Deposit", c => c.Single(nullable: false));
            AddColumn("dbo.Assets", "ProposedBuyerEntityType", c => c.String());
            AddColumn("dbo.Assets", "EquityMargin", c => c.Single(nullable: false));
            AddColumn("dbo.Assets", "NonRetailPricingComparativePercentage", c => c.Single(nullable: false));
            AddColumn("dbo.Assets", "LocalMlsDefinition", c => c.String());
            AddColumn("dbo.Assets", "MlsId", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "ListedByRealtor", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "HasHOA", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "Address", c => c.String());
            DropColumn("dbo.Assets", "ListingAgentCompany");
            DropColumn("dbo.Assets", "DateCommissionToEpiReceived");
            DropColumn("dbo.Assets", "CommissionShareToEPI");
            DropColumn("dbo.Assets", "CommissionShareAgr");
            DropColumn("dbo.Assets", "DateOfCsaConfirm");
            DropColumn("dbo.Assets", "ActualClosingDate");
            DropColumn("dbo.Assets", "PropertyAddress");
            CreateIndex("dbo.Assets", "HOA_AssetHoaId");
            AddForeignKey("dbo.Assets", "HOA_AssetHoaId", "dbo.AssetHoas", "AssetHoaId");
        }
    }
}
