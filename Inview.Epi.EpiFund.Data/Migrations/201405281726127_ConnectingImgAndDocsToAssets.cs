namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectingImgAndDocsToAssets : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AssetDocuments", new[] { "Asset_AssetId" });
            RenameColumn(table: "dbo.AssetDocuments", name: "Asset_AssetId", newName: "AssetId");
            AddColumn("dbo.AssetImages", "AssetId", c => c.Guid(nullable: false));
            AddColumn("dbo.Users", "NCNDSignDate", c => c.DateTime());
            AddColumn("dbo.Users", "NCNDFileLocation", c => c.String());
            AlterColumn("dbo.AssetDocuments", "AssetId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AssetDocuments", "AssetId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AssetDocuments", new[] { "AssetId" });
            AlterColumn("dbo.AssetDocuments", "AssetId", c => c.Guid());
            DropColumn("dbo.Users", "NCNDFileLocation");
            DropColumn("dbo.Users", "NCNDSignDate");
            DropColumn("dbo.AssetImages", "AssetId");
            RenameColumn(table: "dbo.AssetDocuments", name: "AssetId", newName: "Asset_AssetId");
            CreateIndex("dbo.AssetDocuments", "Asset_AssetId");
        }
    }
}
