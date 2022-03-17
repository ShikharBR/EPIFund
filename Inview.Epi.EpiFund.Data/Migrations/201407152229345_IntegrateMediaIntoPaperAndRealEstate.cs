namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntegrateMediaIntoPaperAndRealEstate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetDocuments", "PaperCommercialAsset_PaperCommercialAssetId", c => c.Int());
            AddColumn("dbo.AssetDocuments", "PaperResidentialAsset_PaperResidentialAssetId", c => c.Int());
            AddColumn("dbo.AssetDocuments", "RealEstateCommercialAsset_RealEstateCommercialAssetId", c => c.Int());
            AddColumn("dbo.AssetDocuments", "RealEstateResidentialAsset_RealEstateResidentialAssetId", c => c.Int());
            AddColumn("dbo.AssetImages", "PaperCommercialAsset_PaperCommercialAssetId", c => c.Int());
            AddColumn("dbo.AssetImages", "PaperResidentialAsset_PaperResidentialAssetId", c => c.Int());
            AddColumn("dbo.AssetImages", "RealEstateCommercialAsset_RealEstateCommercialAssetId", c => c.Int());
            AddColumn("dbo.AssetImages", "RealEstateResidentialAsset_RealEstateResidentialAssetId", c => c.Int());
            AddColumn("dbo.AssetSellers", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.AssetVideos", "PaperCommercialAsset_PaperCommercialAssetId", c => c.Int());
            AddColumn("dbo.AssetVideos", "PaperResidentialAsset_PaperResidentialAssetId", c => c.Int());
            AddColumn("dbo.AssetVideos", "RealEstateCommercialAsset_RealEstateCommercialAssetId", c => c.Int());
            AddColumn("dbo.AssetVideos", "RealEstateResidentialAsset_RealEstateResidentialAssetId", c => c.Int());
            AddColumn("dbo.PaperCommercialAssets", "GuidId", c => c.Guid(nullable: false));
            AddColumn("dbo.PaperResidentialAssets", "GuidId", c => c.Guid(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "GuidId", c => c.Guid(nullable: false));
            AddColumn("dbo.RealEstateResidentialAssets", "GuidId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AssetDocuments", "PaperCommercialAsset_PaperCommercialAssetId");
            CreateIndex("dbo.AssetDocuments", "PaperResidentialAsset_PaperResidentialAssetId");
            CreateIndex("dbo.AssetDocuments", "RealEstateCommercialAsset_RealEstateCommercialAssetId");
            CreateIndex("dbo.AssetDocuments", "RealEstateResidentialAsset_RealEstateResidentialAssetId");
            CreateIndex("dbo.AssetImages", "PaperCommercialAsset_PaperCommercialAssetId");
            CreateIndex("dbo.AssetImages", "PaperResidentialAsset_PaperResidentialAssetId");
            CreateIndex("dbo.AssetImages", "RealEstateCommercialAsset_RealEstateCommercialAssetId");
            CreateIndex("dbo.AssetImages", "RealEstateResidentialAsset_RealEstateResidentialAssetId");
            CreateIndex("dbo.AssetVideos", "PaperCommercialAsset_PaperCommercialAssetId");
            CreateIndex("dbo.AssetVideos", "PaperResidentialAsset_PaperResidentialAssetId");
            CreateIndex("dbo.AssetVideos", "RealEstateCommercialAsset_RealEstateCommercialAssetId");
            CreateIndex("dbo.AssetVideos", "RealEstateResidentialAsset_RealEstateResidentialAssetId");
            AddForeignKey("dbo.AssetDocuments", "PaperCommercialAsset_PaperCommercialAssetId", "dbo.PaperCommercialAssets", "PaperCommercialAssetId");
            AddForeignKey("dbo.AssetImages", "PaperCommercialAsset_PaperCommercialAssetId", "dbo.PaperCommercialAssets", "PaperCommercialAssetId");
            AddForeignKey("dbo.AssetVideos", "PaperCommercialAsset_PaperCommercialAssetId", "dbo.PaperCommercialAssets", "PaperCommercialAssetId");
            AddForeignKey("dbo.AssetDocuments", "PaperResidentialAsset_PaperResidentialAssetId", "dbo.PaperResidentialAssets", "PaperResidentialAssetId");
            AddForeignKey("dbo.AssetImages", "PaperResidentialAsset_PaperResidentialAssetId", "dbo.PaperResidentialAssets", "PaperResidentialAssetId");
            AddForeignKey("dbo.AssetVideos", "PaperResidentialAsset_PaperResidentialAssetId", "dbo.PaperResidentialAssets", "PaperResidentialAssetId");
            AddForeignKey("dbo.AssetDocuments", "RealEstateCommercialAsset_RealEstateCommercialAssetId", "dbo.RealEstateCommercialAssets", "RealEstateCommercialAssetId");
            AddForeignKey("dbo.AssetImages", "RealEstateCommercialAsset_RealEstateCommercialAssetId", "dbo.RealEstateCommercialAssets", "RealEstateCommercialAssetId");
            AddForeignKey("dbo.AssetVideos", "RealEstateCommercialAsset_RealEstateCommercialAssetId", "dbo.RealEstateCommercialAssets", "RealEstateCommercialAssetId");
            AddForeignKey("dbo.AssetDocuments", "RealEstateResidentialAsset_RealEstateResidentialAssetId", "dbo.RealEstateResidentialAssets", "RealEstateResidentialAssetId");
            AddForeignKey("dbo.AssetImages", "RealEstateResidentialAsset_RealEstateResidentialAssetId", "dbo.RealEstateResidentialAssets", "RealEstateResidentialAssetId");
            AddForeignKey("dbo.AssetVideos", "RealEstateResidentialAsset_RealEstateResidentialAssetId", "dbo.RealEstateResidentialAssets", "RealEstateResidentialAssetId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetVideos", "RealEstateResidentialAsset_RealEstateResidentialAssetId", "dbo.RealEstateResidentialAssets");
            DropForeignKey("dbo.AssetImages", "RealEstateResidentialAsset_RealEstateResidentialAssetId", "dbo.RealEstateResidentialAssets");
            DropForeignKey("dbo.AssetDocuments", "RealEstateResidentialAsset_RealEstateResidentialAssetId", "dbo.RealEstateResidentialAssets");
            DropForeignKey("dbo.AssetVideos", "RealEstateCommercialAsset_RealEstateCommercialAssetId", "dbo.RealEstateCommercialAssets");
            DropForeignKey("dbo.AssetImages", "RealEstateCommercialAsset_RealEstateCommercialAssetId", "dbo.RealEstateCommercialAssets");
            DropForeignKey("dbo.AssetDocuments", "RealEstateCommercialAsset_RealEstateCommercialAssetId", "dbo.RealEstateCommercialAssets");
            DropForeignKey("dbo.AssetVideos", "PaperResidentialAsset_PaperResidentialAssetId", "dbo.PaperResidentialAssets");
            DropForeignKey("dbo.AssetImages", "PaperResidentialAsset_PaperResidentialAssetId", "dbo.PaperResidentialAssets");
            DropForeignKey("dbo.AssetDocuments", "PaperResidentialAsset_PaperResidentialAssetId", "dbo.PaperResidentialAssets");
            DropForeignKey("dbo.AssetVideos", "PaperCommercialAsset_PaperCommercialAssetId", "dbo.PaperCommercialAssets");
            DropForeignKey("dbo.AssetImages", "PaperCommercialAsset_PaperCommercialAssetId", "dbo.PaperCommercialAssets");
            DropForeignKey("dbo.AssetDocuments", "PaperCommercialAsset_PaperCommercialAssetId", "dbo.PaperCommercialAssets");
            DropIndex("dbo.AssetVideos", new[] { "RealEstateResidentialAsset_RealEstateResidentialAssetId" });
            DropIndex("dbo.AssetVideos", new[] { "RealEstateCommercialAsset_RealEstateCommercialAssetId" });
            DropIndex("dbo.AssetVideos", new[] { "PaperResidentialAsset_PaperResidentialAssetId" });
            DropIndex("dbo.AssetVideos", new[] { "PaperCommercialAsset_PaperCommercialAssetId" });
            DropIndex("dbo.AssetImages", new[] { "RealEstateResidentialAsset_RealEstateResidentialAssetId" });
            DropIndex("dbo.AssetImages", new[] { "RealEstateCommercialAsset_RealEstateCommercialAssetId" });
            DropIndex("dbo.AssetImages", new[] { "PaperResidentialAsset_PaperResidentialAssetId" });
            DropIndex("dbo.AssetImages", new[] { "PaperCommercialAsset_PaperCommercialAssetId" });
            DropIndex("dbo.AssetDocuments", new[] { "RealEstateResidentialAsset_RealEstateResidentialAssetId" });
            DropIndex("dbo.AssetDocuments", new[] { "RealEstateCommercialAsset_RealEstateCommercialAssetId" });
            DropIndex("dbo.AssetDocuments", new[] { "PaperResidentialAsset_PaperResidentialAssetId" });
            DropIndex("dbo.AssetDocuments", new[] { "PaperCommercialAsset_PaperCommercialAssetId" });
            DropColumn("dbo.RealEstateResidentialAssets", "GuidId");
            DropColumn("dbo.RealEstateCommercialAssets", "GuidId");
            DropColumn("dbo.PaperResidentialAssets", "GuidId");
            DropColumn("dbo.PaperCommercialAssets", "GuidId");
            DropColumn("dbo.AssetVideos", "RealEstateResidentialAsset_RealEstateResidentialAssetId");
            DropColumn("dbo.AssetVideos", "RealEstateCommercialAsset_RealEstateCommercialAssetId");
            DropColumn("dbo.AssetVideos", "PaperResidentialAsset_PaperResidentialAssetId");
            DropColumn("dbo.AssetVideos", "PaperCommercialAsset_PaperCommercialAssetId");
            DropColumn("dbo.AssetSellers", "UserId");
            DropColumn("dbo.AssetImages", "RealEstateResidentialAsset_RealEstateResidentialAssetId");
            DropColumn("dbo.AssetImages", "RealEstateCommercialAsset_RealEstateCommercialAssetId");
            DropColumn("dbo.AssetImages", "PaperResidentialAsset_PaperResidentialAssetId");
            DropColumn("dbo.AssetImages", "PaperCommercialAsset_PaperCommercialAssetId");
            DropColumn("dbo.AssetDocuments", "RealEstateResidentialAsset_RealEstateResidentialAssetId");
            DropColumn("dbo.AssetDocuments", "RealEstateCommercialAsset_RealEstateCommercialAssetId");
            DropColumn("dbo.AssetDocuments", "PaperResidentialAsset_PaperResidentialAssetId");
            DropColumn("dbo.AssetDocuments", "PaperCommercialAsset_PaperCommercialAssetId");
        }
    }
}
