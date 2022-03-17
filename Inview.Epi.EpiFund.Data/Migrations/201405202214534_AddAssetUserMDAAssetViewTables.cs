namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssetUserMDAAssetViewTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetUserMDAs",
                c => new
                    {
                        AssetUserMDAId = c.Int(nullable: false, identity: true),
                        AssetId = c.Guid(nullable: false),
                        SignMDADate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssetUserMDAId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.AssetId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AssetUserViews",
                c => new
                    {
                        AssetUserViewId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ViewDate = c.DateTime(nullable: false),
                        Asset_AssetId = c.Guid(),
                    })
                .PrimaryKey(t => t.AssetUserViewId)
                .ForeignKey("dbo.Assets", t => t.Asset_AssetId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.Asset_AssetId);
            
            AddColumn("dbo.Assets", "DaysBeforeListing", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetUserViews", "UserId", "dbo.Users");
            DropForeignKey("dbo.AssetUserViews", "Asset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetUserMDAs", "UserId", "dbo.Users");
            DropForeignKey("dbo.AssetUserMDAs", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetUserViews", new[] { "Asset_AssetId" });
            DropIndex("dbo.AssetUserViews", new[] { "UserId" });
            DropIndex("dbo.AssetUserMDAs", new[] { "UserId" });
            DropIndex("dbo.AssetUserMDAs", new[] { "AssetId" });
            DropColumn("dbo.Assets", "DaysBeforeListing");
            DropTable("dbo.AssetUserViews");
            DropTable("dbo.AssetUserMDAs");
        }
    }
}
