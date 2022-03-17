namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeListofEnums : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "IsTBDMarket", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "FlyerImage_AssetImageId", c => c.Guid());
            CreateIndex("dbo.Assets", "FlyerImage_AssetImageId");
            AddForeignKey("dbo.Assets", "FlyerImage_AssetImageId", "dbo.AssetImages", "AssetImageId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "FlyerImage_AssetImageId", "dbo.AssetImages");
            DropIndex("dbo.Assets", new[] { "FlyerImage_AssetImageId" });
            DropColumn("dbo.Assets", "FlyerImage_AssetImageId");
            DropColumn("dbo.Assets", "IsTBDMarket");
        }
    }
}
