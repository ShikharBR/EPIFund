namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetPK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetDocuments", "Asset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetUnitSpecifications", "MultiFamilyAsset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetCommissions", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetSalePayments", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetUserMDAs", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetUserViews", "AssetId", "dbo.Assets");
            DropPrimaryKey("dbo.Assets");
            AlterColumn("dbo.Assets", "AssetId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetDocuments", "Asset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetUnitSpecifications", "MultiFamilyAsset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetCommissions", "AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetSalePayments", "AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetUserMDAs", "AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetUserViews", "AssetId", "dbo.Assets", "AssetId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetUserViews", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetUserMDAs", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetSalePayments", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetCommissions", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetUnitSpecifications", "MultiFamilyAsset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetDocuments", "Asset_AssetId", "dbo.Assets");
            DropPrimaryKey("dbo.Assets");
            AlterColumn("dbo.Assets", "AssetId", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetUserViews", "AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetUserMDAs", "AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetSalePayments", "AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetCommissions", "AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetUnitSpecifications", "MultiFamilyAsset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetDocuments", "Asset_AssetId", "dbo.Assets", "AssetId");
        }
    }
}
