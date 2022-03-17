namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKAssetImageAndDocs : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AssetDocuments", new[] { "AssetId" });
            RenameColumn(table: "dbo.AssetDocuments", name: "AssetId", newName: "Asset_AssetId");
            AlterColumn("dbo.AssetDocuments", "Asset_AssetId", c => c.Guid());
            CreateIndex("dbo.AssetDocuments", "Asset_AssetId");
            DropColumn("dbo.AssetDocuments", "URL");
            DropColumn("dbo.AssetImages", "AssetId");
            DropColumn("dbo.AssetImages", "Url");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssetImages", "Url", c => c.String());
            AddColumn("dbo.AssetImages", "AssetId", c => c.Guid(nullable: false));
            AddColumn("dbo.AssetDocuments", "URL", c => c.String());
            DropIndex("dbo.AssetDocuments", new[] { "Asset_AssetId" });
            AlterColumn("dbo.AssetDocuments", "Asset_AssetId", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.AssetDocuments", name: "Asset_AssetId", newName: "AssetId");
            CreateIndex("dbo.AssetDocuments", "AssetId");
        }
    }
}
