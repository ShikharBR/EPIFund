namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuctionDateToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "AuctionDate", c => c.DateTime());
            AddColumn("dbo.Assets", "HoldEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "HoldEndDate");
            DropColumn("dbo.Assets", "AuctionDate");
        }
    }
}
