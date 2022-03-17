namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVideoToAssets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetVideos",
                c => new
                    {
                        AssetVideoId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        FilePath = c.String(),
                        AssetId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AssetVideoId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .Index(t => t.AssetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetVideos", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetVideos", new[] { "AssetId" });
            DropTable("dbo.AssetVideos");
        }
    }
}
