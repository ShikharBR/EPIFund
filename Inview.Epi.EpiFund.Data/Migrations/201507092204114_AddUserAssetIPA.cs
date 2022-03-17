namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAssetIPA : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.UserAssetIPAId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AssetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAssetIPAs", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserAssetIPAs", "AssetId", "dbo.Assets");
            DropIndex("dbo.UserAssetIPAs", new[] { "AssetId" });
            DropIndex("dbo.UserAssetIPAs", new[] { "UserId" });
            DropTable("dbo.UserAssetIPAs");
        }
    }
}
