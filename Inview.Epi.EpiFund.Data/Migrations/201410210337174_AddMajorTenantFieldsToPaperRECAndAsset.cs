namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMajorTenantFieldsToPaperRECAndAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "LeasedSquareFootageByMajorTenant", c => c.Int());
            AddColumn("dbo.Assets", "BaseRentPerSqFtMajorTenant", c => c.Double());
            AddColumn("dbo.Assets", "CurrentMarkerRentPerSqFt", c => c.Double());
            AddColumn("dbo.PaperCommercialAssets", "LeasedSquareFootageByMajorTenant", c => c.Int(nullable: false));
            AddColumn("dbo.PaperCommercialAssets", "BaseRentPerSqFtMajorTenant", c => c.Double(nullable: false));
            AddColumn("dbo.PaperCommercialAssets", "CurrentMarkerRentPerSqFt", c => c.Double(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "LeasedSquareFootageByMajorTenant", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "BaseRentPerSqFtMajorTenant", c => c.Double(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "CurrentMarkerRentPerSqFt", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RealEstateCommercialAssets", "CurrentMarkerRentPerSqFt");
            DropColumn("dbo.RealEstateCommercialAssets", "BaseRentPerSqFtMajorTenant");
            DropColumn("dbo.RealEstateCommercialAssets", "LeasedSquareFootageByMajorTenant");
            DropColumn("dbo.PaperCommercialAssets", "CurrentMarkerRentPerSqFt");
            DropColumn("dbo.PaperCommercialAssets", "BaseRentPerSqFtMajorTenant");
            DropColumn("dbo.PaperCommercialAssets", "LeasedSquareFootageByMajorTenant");
            DropColumn("dbo.Assets", "CurrentMarkerRentPerSqFt");
            DropColumn("dbo.Assets", "BaseRentPerSqFtMajorTenant");
            DropColumn("dbo.Assets", "LeasedSquareFootageByMajorTenant");
        }
    }
}
