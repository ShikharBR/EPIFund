namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditBuyerAssetFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ProposedBuyerID", c => c.String());
            DropColumn("dbo.Assets", "ProposedNewBuyerEmail");
            DropColumn("dbo.Assets", "ProposedBuyerWorkPhone");
            DropColumn("dbo.Assets", "ProposedBuyerCellPhone");
            DropColumn("dbo.Assets", "ProposedBuyerFax");
            DropColumn("dbo.Assets", "NotinBuyerList");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "NotinBuyerList", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "ProposedBuyerFax", c => c.String());
            AddColumn("dbo.Assets", "ProposedBuyerCellPhone", c => c.String());
            AddColumn("dbo.Assets", "ProposedBuyerWorkPhone", c => c.String());
            AddColumn("dbo.Assets", "ProposedNewBuyerEmail", c => c.String());
            DropColumn("dbo.Assets", "ProposedBuyerID");
        }
    }
}
