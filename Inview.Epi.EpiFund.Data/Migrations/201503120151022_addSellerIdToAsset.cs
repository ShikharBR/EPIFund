namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSellerIdToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "AssetSellerId", c => c.Int());
            DropColumn("dbo.Assets", "SubmittedBySeller");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "SubmittedBySeller", c => c.Boolean());
            DropColumn("dbo.Assets", "AssetSellerId");
        }
    }
}
