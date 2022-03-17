namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropListingAgents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetListingAgents", "NarMemberId", "dbo.NARMembers");
            DropForeignKey("dbo.AssetListingAgents", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetListingAgents", new[] { "AssetId" });
            DropIndex("dbo.AssetListingAgents", new[] { "NarMemberId" });
            DropTable("dbo.AssetListingAgents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AssetListingAgents",
                c => new
                    {
                        AssetListingAgentId = c.Int(nullable: false, identity: true),
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
                        NarMemberId = c.Int(),
                        CommissionShareAgr = c.Boolean(nullable: false),
                        DateOfCsaConfirm = c.DateTime(),
                        NotOnList = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AssetListingAgentId);
            
            CreateIndex("dbo.AssetListingAgents", "NarMemberId");
            CreateIndex("dbo.AssetListingAgents", "AssetId");
            AddForeignKey("dbo.AssetListingAgents", "AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetListingAgents", "NarMemberId", "dbo.NARMembers", "NarMemberId");
        }
    }
}
