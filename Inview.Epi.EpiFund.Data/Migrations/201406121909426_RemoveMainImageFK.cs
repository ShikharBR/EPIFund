namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMainImageFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "MainImage_AssetImageId", "dbo.AssetImages");
            DropIndex("dbo.Assets", new[] { "MainImage_AssetImageId" });
            DropIndex("dbo.AssetImages", new[] { "Asset_AssetId" });
            DropTable("dbo.AssetImages");
            CreateTable(
                "dbo.AssetImages",
                c => new
                {
                    AssetImageId = c.Guid(nullable: false),
                    FileName = c.String(),
                    ContentType = c.String(),
                    Order = c.Int(nullable: false),
                    AssetId = c.Guid(nullable: false),
                    IsFlyerImage = c.Boolean(nullable: false, defaultValue: false),
                })
                .PrimaryKey(t => t.AssetImageId)
                .ForeignKey("dbo.Assets", t => t.AssetId);
        }
        
        public override void Down()
        {
            DropTable("dbo.AssetImages");
        }
    }
}
