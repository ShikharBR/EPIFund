namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeUserIdNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AssetSearchCriterias", new[] { "UserId" });
            AlterColumn("dbo.AssetSearchCriterias", "UserId", c => c.Int());
            CreateIndex("dbo.AssetSearchCriterias", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AssetSearchCriterias", new[] { "UserId" });
            AlterColumn("dbo.AssetSearchCriterias", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.AssetSearchCriterias", "UserId");
        }
    }
}
