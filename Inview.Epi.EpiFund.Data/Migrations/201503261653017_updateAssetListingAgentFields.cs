namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAssetListingAgentFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetListingAgents", "ListingAgentCorpAddress2", c => c.String());
            AddColumn("dbo.AssetListingAgents", "ListingAgentCity", c => c.String());
            AddColumn("dbo.AssetListingAgents", "ListingAgentState", c => c.String());
            AddColumn("dbo.AssetListingAgents", "ListingAgentZip", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetListingAgents", "ListingAgentZip");
            DropColumn("dbo.AssetListingAgents", "ListingAgentState");
            DropColumn("dbo.AssetListingAgents", "ListingAgentCity");
            DropColumn("dbo.AssetListingAgents", "ListingAgentCorpAddress2");
        }
    }
}
