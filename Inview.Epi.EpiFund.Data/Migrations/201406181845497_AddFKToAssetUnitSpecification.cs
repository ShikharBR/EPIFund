namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKToAssetUnitSpecification : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AssetUnitSpecifications", new[] { "MultiFamilyAsset_AssetId" });
            RenameColumn(table: "dbo.AssetUnitSpecifications", name: "MultiFamilyAsset_AssetId", newName: "AssetId");
            AlterColumn("dbo.AssetUnitSpecifications", "AssetId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AssetUnitSpecifications", "AssetId");
            AddForeignKey("dbo.AssetUnitSpecifications", "AssetId", "dbo.Assets", "AssetId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetUnitSpecifications", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetUnitSpecifications", new[] { "AssetId" });
            AlterColumn("dbo.AssetUnitSpecifications", "AssetId", c => c.Guid());
            RenameColumn(table: "dbo.AssetUnitSpecifications", name: "AssetId", newName: "MultiFamilyAsset_AssetId");
            CreateIndex("dbo.AssetUnitSpecifications", "MultiFamilyAsset_AssetId");
        }
    }
}
