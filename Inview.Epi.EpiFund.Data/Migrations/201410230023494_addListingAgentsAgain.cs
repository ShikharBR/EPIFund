namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addListingAgentsAgain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetListingAgents",
                c => new
                    {
                        AssetListingAgentId = c.Guid(nullable: false),
                        AssetId = c.Guid(nullable: false),
                        ListingAgentCompany = c.String(),
                        ListingAgentName = c.String(),
                        ListingAgentEmail = c.String(),
                        ListingAgentPhoneNumber = c.String(),
                        ListingAgentCorpAddress = c.String(),
                        ListingAgentCommissionAmount = c.String(),
                        ListingAgentNewName = c.String(),
                        ListingAgentWorkNumber = c.String(),
                        ListingAgentCellNumber = c.String(),
                        ListingAgentFaxNumber = c.String(),
                        CommissionShareAgr = c.Boolean(nullable: false),
                        DateOfCsaConfirm = c.DateTime(),
                        NotOnList = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AssetListingAgentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AssetListingAgents");
        }
    }
}
