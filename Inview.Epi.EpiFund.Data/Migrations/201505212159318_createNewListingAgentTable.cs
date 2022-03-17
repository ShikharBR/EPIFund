namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createNewListingAgentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetNARMembers",
                c => new
                    {
                        AssetNARMemberId = c.Guid(nullable: false),
                        AssetId = c.Guid(nullable: false),
                        NarMemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssetNARMemberId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .ForeignKey("dbo.NARMembers", t => t.NarMemberId)
                .Index(t => t.AssetId)
                .Index(t => t.NarMemberId);
            
            AddColumn("dbo.UserReferrals", "Registered", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetNARMembers", "NarMemberId", "dbo.NARMembers");
            DropForeignKey("dbo.AssetNARMembers", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetNARMembers", new[] { "NarMemberId" });
            DropIndex("dbo.AssetNARMembers", new[] { "AssetId" });
            DropColumn("dbo.UserReferrals", "Registered");
            DropTable("dbo.AssetNARMembers");
        }
    }
}
