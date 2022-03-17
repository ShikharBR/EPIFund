namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTempDeferredmaintenanceAndREComrclExtraFields : DbMigration
    {
        public override void Up()
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
            
            AddColumn("dbo.RealEstateCommercialAssets", "CorporateName", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "CellPhone", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "AcronymForCorporateEntity", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "CorporateEntityType", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "CorporateTitle", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "StateOfOriginCorporateEntity", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "IsCertificateOfGoodStandingAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "PreferredMethods", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "PreferredContactTime", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "CorporateAddress1", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "CorporateAddress2", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "City", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "State", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "Zip", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "PropertyCounty", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "TaxAssessorNumber", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "TaxAssessorNumberOther", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "TotalRentableFeet", c => c.Int());
            AddColumn("dbo.RealEstateCommercialAssets", "MortgageLienType", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "MortgageLienAssumable", c => c.Boolean(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "TotalRentableFeetAllApt", c => c.Int());
            AddColumn("dbo.RealEstateCommercialAssets", "AnnualPropertyTaxes", c => c.Double());
            DropColumn("dbo.RealEstateCommercialAssets", "HomePhone");
            DropColumn("dbo.RealEstateCommercialAssets", "OtherPhone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RealEstateCommercialAssets", "OtherPhone", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "HomePhone", c => c.String());
            DropColumn("dbo.RealEstateCommercialAssets", "AnnualPropertyTaxes");
            DropColumn("dbo.RealEstateCommercialAssets", "TotalRentableFeetAllApt");
            DropColumn("dbo.RealEstateCommercialAssets", "MortgageLienAssumable");
            DropColumn("dbo.RealEstateCommercialAssets", "MortgageLienType");
            DropColumn("dbo.RealEstateCommercialAssets", "TotalRentableFeet");
            DropColumn("dbo.RealEstateCommercialAssets", "TaxAssessorNumberOther");
            DropColumn("dbo.RealEstateCommercialAssets", "TaxAssessorNumber");
            DropColumn("dbo.RealEstateCommercialAssets", "PropertyCounty");
            DropColumn("dbo.RealEstateCommercialAssets", "Zip");
            DropColumn("dbo.RealEstateCommercialAssets", "State");
            DropColumn("dbo.RealEstateCommercialAssets", "City");
            DropColumn("dbo.RealEstateCommercialAssets", "CorporateAddress2");
            DropColumn("dbo.RealEstateCommercialAssets", "CorporateAddress1");
            DropColumn("dbo.RealEstateCommercialAssets", "PreferredContactTime");
            DropColumn("dbo.RealEstateCommercialAssets", "PreferredMethods");
            DropColumn("dbo.RealEstateCommercialAssets", "IsCertificateOfGoodStandingAvailable");
            DropColumn("dbo.RealEstateCommercialAssets", "StateOfOriginCorporateEntity");
            DropColumn("dbo.RealEstateCommercialAssets", "CorporateTitle");
            DropColumn("dbo.RealEstateCommercialAssets", "CorporateEntityType");
            DropColumn("dbo.RealEstateCommercialAssets", "AcronymForCorporateEntity");
            DropColumn("dbo.RealEstateCommercialAssets", "CellPhone");
            DropColumn("dbo.RealEstateCommercialAssets", "CorporateName");
            DropTable("dbo.TemporaryDeferredMaintenanceItems");
        }
    }
}
