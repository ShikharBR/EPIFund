namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeAssetListingAgents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetListingAgents", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetListingAgents", new[] { "AssetId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.AssetListingAgents", "AssetId");
            AddForeignKey("dbo.AssetListingAgents", "AssetId", "dbo.Assets", "AssetId");
        }
    }
}
