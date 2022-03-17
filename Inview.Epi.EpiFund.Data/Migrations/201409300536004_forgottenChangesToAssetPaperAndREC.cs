namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forgottenChangesToAssetPaperAndREC : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.RealEstateCommercialAssets", "PropertyDetailsString");
            //DropColumn("dbo.RealEstateCommercialAssets", "OccupancyDate");
            //DropColumn("dbo.RealEstateCommercialAssets", "ProformaAnnualNoi");
            //DropColumn("dbo.RealEstateCommercialAssets", "ProformaSgi");
            //DropColumn("dbo.PaperCommercialAssets", "PreferredContactTimes");
            //DropColumn("dbo.PaperCommercialAssets", "PreferredMethods");
            //DropColumn("dbo.PaperCommercialAssets", "PropertyDetailsString");
            //DropColumn("dbo.PaperCommercialAssets", "OccupancyDate");
            //DropColumn("dbo.PaperCommercialAssets", "ProformaAnnualNoi");
            //DropColumn("dbo.PaperCommercialAssets", "ProformaSgi");
            //DropColumn("dbo.PaperCommercialAssets", "RentableSquareFeet");
            //DropColumn("dbo.PaperCommercialAssets", "MFDetailsString");
            //DropColumn("dbo.PaperCommercialAssets", "MortgageLienAssumable");
            //DropColumn("dbo.PaperCommercialAssets", "MortgageLienType");
            //DropColumn("dbo.PaperCommercialAssets", "HasPositionMortgage");
            AddColumn("dbo.PaperCommercialAssets", "HasPositionMortgage", c => c.Int(nullable: false));
            AddColumn("dbo.PaperCommercialAssets", "MortgageLienType", c => c.Int());
            AddColumn("dbo.PaperCommercialAssets", "MortgageLienAssumable", c => c.Int());
            AddColumn("dbo.PaperCommercialAssets", "MFDetailsString", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "RentableSquareFeet", c => c.Int(nullable: false));
            AddColumn("dbo.PaperCommercialAssets", "ProformaSgi", c => c.Int(nullable: false));
            AddColumn("dbo.PaperCommercialAssets", "ProformaAnnualNoi", c => c.Int(nullable: false));
            AddColumn("dbo.PaperCommercialAssets", "OccupancyDate", c => c.DateTime());
            AddColumn("dbo.PaperCommercialAssets", "PropertyDetailsString", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "PreferredMethods", c => c.String());
            AddColumn("dbo.PaperCommercialAssets", "PreferredContactTimes", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "ProformaSgi", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "ProformaAnnualNoi", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "OccupancyDate", c => c.DateTime());
            AddColumn("dbo.RealEstateCommercialAssets", "PropertyDetailsString", c => c.String());
            //DropColumn("dbo.Assets", "TypeOfMortgage");
            //DropColumn("dbo.PaperCommercialAssets", "TypeOfMortgage");
            //DropColumn("dbo.RealEstateCommercialAssets", "TypeOfMortgage");
        }

        public override void Down()
        {
            //AddColumn("dbo.RealEstateCommercialAssets", "TypeOfMortgage", c => c.String());
            //AddColumn("dbo.PaperCommercialAssets", "TypeOfMortgage", c => c.String());
            //AddColumn("dbo.Assets", "TypeOfMortgage", c => c.String());
            DropColumn("dbo.RealEstateCommercialAssets", "PropertyDetailsString");
            DropColumn("dbo.RealEstateCommercialAssets", "OccupancyDate");
            DropColumn("dbo.RealEstateCommercialAssets", "ProformaAnnualNoi");
            DropColumn("dbo.RealEstateCommercialAssets", "ProformaSgi");
            DropColumn("dbo.PaperCommercialAssets", "PreferredContactTimes");
            DropColumn("dbo.PaperCommercialAssets", "PreferredMethods");
            DropColumn("dbo.PaperCommercialAssets", "PropertyDetailsString");
            DropColumn("dbo.PaperCommercialAssets", "OccupancyDate");
            DropColumn("dbo.PaperCommercialAssets", "ProformaAnnualNoi");
            DropColumn("dbo.PaperCommercialAssets", "ProformaSgi");
            DropColumn("dbo.PaperCommercialAssets", "RentableSquareFeet");
            DropColumn("dbo.PaperCommercialAssets", "MFDetailsString");
            DropColumn("dbo.PaperCommercialAssets", "MortgageLienAssumable");
            DropColumn("dbo.PaperCommercialAssets", "MortgageLienType");
            DropColumn("dbo.PaperCommercialAssets", "HasPositionMortgage");
        }
    }
}
