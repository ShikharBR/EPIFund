namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTempMedia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetDocuments", "PaperCommercialAsset_PaperCommercialAssetId", "dbo.PaperCommercialAssets");
            DropForeignKey("dbo.AssetImages", "PaperCommercialAsset_PaperCommercialAssetId", "dbo.PaperCommercialAssets");
            DropForeignKey("dbo.AssetVideos", "PaperCommercialAsset_PaperCommercialAssetId", "dbo.PaperCommercialAssets");
            DropForeignKey("dbo.AssetDocuments", "PaperResidentialAsset_PaperResidentialAssetId", "dbo.PaperResidentialAssets");
            DropForeignKey("dbo.AssetImages", "PaperResidentialAsset_PaperResidentialAssetId", "dbo.PaperResidentialAssets");
            DropForeignKey("dbo.AssetVideos", "PaperResidentialAsset_PaperResidentialAssetId", "dbo.PaperResidentialAssets");
            DropForeignKey("dbo.AssetDocuments", "RealEstateCommercialAsset_RealEstateCommercialAssetId", "dbo.RealEstateCommercialAssets");
            DropForeignKey("dbo.AssetImages", "RealEstateCommercialAsset_RealEstateCommercialAssetId", "dbo.RealEstateCommercialAssets");
            DropForeignKey("dbo.AssetVideos", "RealEstateCommercialAsset_RealEstateCommercialAssetId", "dbo.RealEstateCommercialAssets");
            DropForeignKey("dbo.AssetDocuments", "RealEstateResidentialAsset_RealEstateResidentialAssetId", "dbo.RealEstateResidentialAssets");
            DropForeignKey("dbo.AssetImages", "RealEstateResidentialAsset_RealEstateResidentialAssetId", "dbo.RealEstateResidentialAssets");
            DropForeignKey("dbo.AssetVideos", "RealEstateResidentialAsset_RealEstateResidentialAssetId", "dbo.RealEstateResidentialAssets");
            DropIndex("dbo.AssetDocuments", new[] { "PaperCommercialAsset_PaperCommercialAssetId" });
            DropIndex("dbo.AssetDocuments", new[] { "PaperResidentialAsset_PaperResidentialAssetId" });
            DropIndex("dbo.AssetDocuments", new[] { "RealEstateCommercialAsset_RealEstateCommercialAssetId" });
            DropIndex("dbo.AssetDocuments", new[] { "RealEstateResidentialAsset_RealEstateResidentialAssetId" });
            DropIndex("dbo.AssetImages", new[] { "PaperCommercialAsset_PaperCommercialAssetId" });
            DropIndex("dbo.AssetImages", new[] { "PaperResidentialAsset_PaperResidentialAssetId" });
            DropIndex("dbo.AssetImages", new[] { "RealEstateCommercialAsset_RealEstateCommercialAssetId" });
            DropIndex("dbo.AssetImages", new[] { "RealEstateResidentialAsset_RealEstateResidentialAssetId" });
            DropIndex("dbo.AssetVideos", new[] { "PaperCommercialAsset_PaperCommercialAssetId" });
            DropIndex("dbo.AssetVideos", new[] { "PaperResidentialAsset_PaperResidentialAssetId" });
            DropIndex("dbo.AssetVideos", new[] { "RealEstateCommercialAsset_RealEstateCommercialAssetId" });
            DropIndex("dbo.AssetVideos", new[] { "RealEstateResidentialAsset_RealEstateResidentialAssetId" });
            CreateTable(
                "dbo.TempAssetDocuments",
                c => new
                    {
                        TempAssetDocumentId = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ContentType = c.String(),
                        Title = c.String(),
                        Size = c.String(),
                        Order = c.Int(nullable: false),
                        GuidId = c.Guid(nullable: false),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TempAssetDocumentId);
            
            CreateTable(
                "dbo.TempAssetImages",
                c => new
                    {
                        TempAssetImageId = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ContentType = c.String(),
                        Order = c.Int(nullable: false),
                        GuidId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TempAssetImageId);
            
            CreateTable(
                "dbo.TempAssetVideos",
                c => new
                    {
                        TempAssetVideoId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        FilePath = c.String(),
                        GuidId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TempAssetVideoId);
            
            DropColumn("dbo.AssetDocuments", "PaperCommercialAsset_PaperCommercialAssetId");
            DropColumn("dbo.AssetDocuments", "PaperResidentialAsset_PaperResidentialAssetId");
            DropColumn("dbo.AssetDocuments", "RealEstateCommercialAsset_RealEstateCommercialAssetId");
            DropColumn("dbo.AssetDocuments", "RealEstateResidentialAsset_RealEstateResidentialAssetId");
            DropColumn("dbo.AssetImages", "PaperCommercialAsset_PaperCommercialAssetId");
            DropColumn("dbo.AssetImages", "PaperResidentialAsset_PaperResidentialAssetId");
            DropColumn("dbo.AssetImages", "RealEstateCommercialAsset_RealEstateCommercialAssetId");
            DropColumn("dbo.AssetImages", "RealEstateResidentialAsset_RealEstateResidentialAssetId");
            DropColumn("dbo.AssetVideos", "PaperCommercialAsset_PaperCommercialAssetId");
            DropColumn("dbo.AssetVideos", "PaperResidentialAsset_PaperResidentialAssetId");
            DropColumn("dbo.AssetVideos", "RealEstateCommercialAsset_RealEstateCommercialAssetId");
            DropColumn("dbo.AssetVideos", "RealEstateResidentialAsset_RealEstateResidentialAssetId");
        }
        
        public override void Down()
        {
            DropTable("dbo.TempAssetVideos");
            DropTable("dbo.TempAssetImages");
            DropTable("dbo.TempAssetDocuments");
        }
    }
}
