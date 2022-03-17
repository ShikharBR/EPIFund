namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssetSellerIdToAssets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaperCommercialAssets", "AssetSellerId", c => c.Int(nullable: false));
            AddColumn("dbo.PaperResidentialAssets", "AssetSellerId", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "AssetSellerId", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateResidentialAssets", "AssetSellerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RealEstateResidentialAssets", "AssetSellerId");
            DropColumn("dbo.RealEstateCommercialAssets", "AssetSellerId");
            DropColumn("dbo.PaperResidentialAssets", "AssetSellerId");
            DropColumn("dbo.PaperCommercialAssets", "AssetSellerId");
        }
    }
}
