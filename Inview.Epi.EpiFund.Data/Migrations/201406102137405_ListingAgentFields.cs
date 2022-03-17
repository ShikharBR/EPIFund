namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ListingAgentFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ListingAgentPhoneNumber", c => c.String());
            AddColumn("dbo.Assets", "ListingAgentCorpAddress", c => c.String());
            AddColumn("dbo.Assets", "ListingAgentCommissionAmount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "ListingAgentCommissionAmount");
            DropColumn("dbo.Assets", "ListingAgentCorpAddress");
            DropColumn("dbo.Assets", "ListingAgentPhoneNumber");
        }
    }
}
