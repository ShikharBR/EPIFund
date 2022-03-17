namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAgentsToAsset : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AssetListingAgents", "AssetId");
            AddForeignKey("dbo.AssetListingAgents", "AssetId", "dbo.Assets", "AssetId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetListingAgents", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetListingAgents", new[] { "AssetId" });
        }
    }
}
