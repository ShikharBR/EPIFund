namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSection3FieldsToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "HasPositionMortgage", c => c.Boolean());
            AddColumn("dbo.Assets", "MortgageLienType", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "MortgageLienAssumable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "FirstMortgageCompany", c => c.String());
            AddColumn("dbo.Assets", "MortgageCompanyAddress", c => c.String());
            AddColumn("dbo.Assets", "MortgageCompanyCity", c => c.String());
            AddColumn("dbo.Assets", "MortgageCompanyState", c => c.String());
            AddColumn("dbo.Assets", "MortgageCompanyZip", c => c.String());
            AddColumn("dbo.Assets", "LenderPhone", c => c.String());
            AddColumn("dbo.Assets", "LenderPhoneOther", c => c.String());
            AddColumn("dbo.Assets", "AccountNumber", c => c.String());
            AddColumn("dbo.Assets", "TypeOfMortgage", c => c.String());
            AddColumn("dbo.Assets", "CurrentPrincipalBalance", c => c.Double());
            AddColumn("dbo.Assets", "MonthlyPayment", c => c.Double());
            AddColumn("dbo.Assets", "PaymentIncludes", c => c.String());
            AddColumn("dbo.Assets", "InterestRate", c => c.Int());
            AddColumn("dbo.Assets", "IsMortgageAnARM", c => c.Boolean());
            AddColumn("dbo.Assets", "MortgageAdjustIfARM", c => c.String());
            AddColumn("dbo.Assets", "NumberOfMissedPayments", c => c.Int());
            AddColumn("dbo.RealEstateCommercialAssets", "HasPositionMortgage", c => c.Boolean());
            AddColumn("dbo.RealEstateCommercialAssets", "MFDetailsString", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "HasDeferredMaintenance", c => c.Boolean(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "EstDeferredMaintenance", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "AverageAdjustmentToBaseRent", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RealEstateCommercialAssets", "AverageAdjustmentToBaseRent");
            DropColumn("dbo.RealEstateCommercialAssets", "EstDeferredMaintenance");
            DropColumn("dbo.RealEstateCommercialAssets", "HasDeferredMaintenance");
            DropColumn("dbo.RealEstateCommercialAssets", "MFDetailsString");
            DropColumn("dbo.RealEstateCommercialAssets", "HasPositionMortgage");
            DropColumn("dbo.Assets", "NumberOfMissedPayments");
            DropColumn("dbo.Assets", "MortgageAdjustIfARM");
            DropColumn("dbo.Assets", "IsMortgageAnARM");
            DropColumn("dbo.Assets", "InterestRate");
            DropColumn("dbo.Assets", "PaymentIncludes");
            DropColumn("dbo.Assets", "MonthlyPayment");
            DropColumn("dbo.Assets", "CurrentPrincipalBalance");
            DropColumn("dbo.Assets", "TypeOfMortgage");
            DropColumn("dbo.Assets", "AccountNumber");
            DropColumn("dbo.Assets", "LenderPhoneOther");
            DropColumn("dbo.Assets", "LenderPhone");
            DropColumn("dbo.Assets", "MortgageCompanyZip");
            DropColumn("dbo.Assets", "MortgageCompanyState");
            DropColumn("dbo.Assets", "MortgageCompanyCity");
            DropColumn("dbo.Assets", "MortgageCompanyAddress");
            DropColumn("dbo.Assets", "FirstMortgageCompany");
            DropColumn("dbo.Assets", "MortgageLienAssumable");
            DropColumn("dbo.Assets", "MortgageLienType");
            DropColumn("dbo.Assets", "HasPositionMortgage");
        }
    }
}
