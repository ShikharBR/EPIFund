namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GiveGuidAndNumberToAsset : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetDocuments", "Asset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetUnitSpecifications", "MultiFamilyAsset_AssetId", "dbo.Assets");
            DropPrimaryKey("dbo.Assets");
            AddColumn("dbo.Assets", "AssetNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "AssetId", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetDocuments", "Asset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetUnitSpecifications", "MultiFamilyAsset_AssetId", "dbo.Assets", "AssetId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetUnitSpecifications", "MultiFamilyAsset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetDocuments", "Asset_AssetId", "dbo.Assets");
            DropPrimaryKey("dbo.Assets");
            AlterColumn("dbo.Assets", "AssetId", c => c.Guid(nullable: false));
            DropColumn("dbo.Assets", "AssetNumber");
            AddPrimaryKey("dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetUnitSpecifications", "MultiFamilyAsset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetDocuments", "Asset_AssetId", "dbo.Assets", "AssetId");
        }
    }
}
