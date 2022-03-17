namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssetUserDisclosures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetUserDisclosures",
                c => new
                    {
                        AssetUserDisclosureId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AssetId = c.Guid(nullable: false),
                        DateConfirmed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AssetUserDisclosureId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .Index(t => t.AssetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetUserDisclosures", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetUserDisclosures", new[] { "AssetId" });
            DropTable("dbo.AssetUserDisclosures");
        }
    }
}
