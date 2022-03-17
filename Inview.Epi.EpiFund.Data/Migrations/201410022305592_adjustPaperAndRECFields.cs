namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustPaperAndRECFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaperCommercialAssets", "AcronymForCorporateEntity", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "CorporateEntityType", c => c.Int(nullable: false));
            AddColumn("dbo.PaperCommercialAssets", "CorporateTitle", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "StateOfOriginCorporateEntity", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "IsCertificateOfGoodStandingAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaperCommercialAssets", "CorporateAddress1", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "CorporateAddress2", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "City", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "State", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "Zip", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "AskingSalePrice", c => c.Double());
            AddColumn("dbo.PaperCommercialAssets", "Terms", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "MotivationToLiquidate", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "HasEnvironmentalIssues", c => c.Boolean());
            AddColumn("dbo.RealEstateCommercialAssets", "EnvironmentalIssues", c => c.String());
            DropColumn("dbo.PaperCommercialAssets", "HasPicturesOfProperty");
            DropColumn("dbo.PaperCommercialAssets", "GeneralComments");
            DropColumn("dbo.PaperCommercialAssets", "ResellingReason");
            DropColumn("dbo.PaperCommercialAssets", "AskingPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaperCommercialAssets", "AskingPrice", c => c.Double());
            AddColumn("dbo.PaperCommercialAssets", "ResellingReason", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "GeneralComments", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "HasPicturesOfProperty", c => c.Boolean());
            DropColumn("dbo.RealEstateCommercialAssets", "EnvironmentalIssues");
            DropColumn("dbo.RealEstateCommercialAssets", "HasEnvironmentalIssues");
            DropColumn("dbo.PaperCommercialAssets", "MotivationToLiquidate");
            DropColumn("dbo.PaperCommercialAssets", "Terms");
            DropColumn("dbo.PaperCommercialAssets", "AskingSalePrice");
            DropColumn("dbo.PaperCommercialAssets", "Zip");
            DropColumn("dbo.PaperCommercialAssets", "State");
            DropColumn("dbo.PaperCommercialAssets", "City");
            DropColumn("dbo.PaperCommercialAssets", "CorporateAddress2");
            DropColumn("dbo.PaperCommercialAssets", "CorporateAddress1");
            DropColumn("dbo.PaperCommercialAssets", "IsCertificateOfGoodStandingAvailable");
            DropColumn("dbo.PaperCommercialAssets", "StateOfOriginCorporateEntity");
            DropColumn("dbo.PaperCommercialAssets", "CorporateTitle");
            DropColumn("dbo.PaperCommercialAssets", "CorporateEntityType");
            DropColumn("dbo.PaperCommercialAssets", "AcronymForCorporateEntity");
        }
    }
}
