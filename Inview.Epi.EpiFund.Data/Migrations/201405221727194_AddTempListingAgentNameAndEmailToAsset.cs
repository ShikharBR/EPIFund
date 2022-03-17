namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTempListingAgentNameAndEmailToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ListingAgentName", c => c.String());
            AddColumn("dbo.Assets", "ListingAgentEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "ListingAgentEmail");
            DropColumn("dbo.Assets", "ListingAgentName");
        }
    }
}
