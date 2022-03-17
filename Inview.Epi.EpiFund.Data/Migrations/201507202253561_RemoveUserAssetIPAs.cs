namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUserAssetIPAs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserAssetIPAs", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.UserAssetIPAs", "UserId", "dbo.Users");
            DropIndex("dbo.UserAssetIPAs", new[] { "UserId" });
            DropIndex("dbo.UserAssetIPAs", new[] { "AssetId" });
            DropTable("dbo.UserAssetIPAs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserAssetIPAs",
                c => new
                    {
                        UserAssetIPAId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AssetId = c.Guid(nullable: false),
                        FileLocation = c.String(),
                        SignIPADate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserAssetIPAId);
            
            CreateIndex("dbo.UserAssetIPAs", "AssetId");
            CreateIndex("dbo.UserAssetIPAs", "UserId");
            AddForeignKey("dbo.UserAssetIPAs", "UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.UserAssetIPAs", "AssetId", "dbo.Assets", "AssetId");
        }
    }
}
