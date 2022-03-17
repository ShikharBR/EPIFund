namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserToAssetSearchCriteria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetSearchCriterias", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.AssetSearchCriterias", "UserId");
            AddForeignKey("dbo.AssetSearchCriterias", "UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetSearchCriterias", "UserId", "dbo.Users");
            DropIndex("dbo.AssetSearchCriterias", new[] { "UserId" });
            DropColumn("dbo.AssetSearchCriterias", "UserId");
        }
    }
}
