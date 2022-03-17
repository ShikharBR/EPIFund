namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetImages", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetImages", new[] { "AssetId" });
            DropIndex("dbo.AssetImages", new[] { "Asset_AssetId" });
            DropColumn("dbo.AssetImages", "AssetId");
            RenameColumn(table: "dbo.AssetImages", name: "Asset_AssetId", newName: "AssetId");
            AlterColumn("dbo.AssetImages", "AssetId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AssetImages", "AssetId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AssetImages", new[] { "AssetId" });
            AlterColumn("dbo.AssetImages", "AssetId", c => c.Guid());
            RenameColumn(table: "dbo.AssetImages", name: "AssetId", newName: "Asset_AssetId");
            AddColumn("dbo.AssetImages", "AssetId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AssetImages", "Asset_AssetId");
            CreateIndex("dbo.AssetImages", "AssetId");
            AddForeignKey("dbo.AssetImages", "AssetId", "dbo.Assets", "AssetId");
        }
    }
}
