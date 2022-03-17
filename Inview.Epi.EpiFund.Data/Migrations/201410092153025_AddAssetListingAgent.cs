namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssetListingAgent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "NarMemberId", "dbo.NARMembers");
            DropIndex("dbo.Assets", new[] { "NarMemberId" });
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
                    })
                .PrimaryKey(t => t.AssetListingAgentId)
                .ForeignKey("dbo.NARMembers", t => t.NarMemberId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .Index(t => t.AssetId)
                .Index(t => t.NarMemberId);
            
            DropColumn("dbo.Assets", "ListingAgentCompany");
            DropColumn("dbo.Assets", "ListingAgentName");
            DropColumn("dbo.Assets", "ListingAgentEmail");
            DropColumn("dbo.Assets", "ListingAgentPhoneNumber");
            DropColumn("dbo.Assets", "ListingAgentCorpAddress");
            DropColumn("dbo.Assets", "ListingAgentCommissionAmount");
            DropColumn("dbo.Assets", "NarMemberId");
            DropColumn("dbo.Assets", "CommissionShareAgr");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "CommissionShareAgr", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "NarMemberId", c => c.Int());
            AddColumn("dbo.Assets", "ListingAgentCommissionAmount", c => c.String());
            AddColumn("dbo.Assets", "ListingAgentCorpAddress", c => c.String());
            AddColumn("dbo.Assets", "ListingAgentPhoneNumber", c => c.String());
            AddColumn("dbo.Assets", "ListingAgentEmail", c => c.String());
            AddColumn("dbo.Assets", "ListingAgentName", c => c.String());
            AddColumn("dbo.Assets", "ListingAgentCompany", c => c.String());
            DropForeignKey("dbo.AssetListingAgents", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetListingAgents", "NarMemberId", "dbo.NARMembers");
            DropIndex("dbo.AssetListingAgents", new[] { "NarMemberId" });
            DropIndex("dbo.AssetListingAgents", new[] { "AssetId" });
            DropTable("dbo.AssetListingAgents");
            CreateIndex("dbo.Assets", "NarMemberId");
            AddForeignKey("dbo.Assets", "NarMemberId", "dbo.NARMembers", "NarMemberId");
        }
    }
}
