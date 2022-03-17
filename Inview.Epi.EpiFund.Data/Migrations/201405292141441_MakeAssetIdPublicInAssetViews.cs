namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeAssetIdPublicInAssetViews : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AssetUserViews", new[] { "Asset_AssetId" });
            RenameColumn(table: "dbo.AssetUserViews", name: "Asset_AssetId", newName: "AssetId");
            AlterColumn("dbo.AssetUserViews", "AssetId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AssetUserViews", "AssetId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AssetUserViews", new[] { "AssetId" });
            AlterColumn("dbo.AssetUserViews", "AssetId", c => c.Guid());
            RenameColumn(table: "dbo.AssetUserViews", name: "AssetId", newName: "Asset_AssetId");
            CreateIndex("dbo.AssetUserViews", "Asset_AssetId");
        }
    }
}
