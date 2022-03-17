namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ProposedBuyerNewFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ProposedBuyerWorkPhone", c => c.String());
            AddColumn("dbo.Assets", "ProposedBuyerCellPhone", c => c.String());
            AddColumn("dbo.Assets", "ProposedBuyerFax", c => c.String());
            AddColumn("dbo.Assets", "NotinBuyerList", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "ProposedNewBuyerEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "NotinBuyerList");
            DropColumn("dbo.Assets", "ProposedBuyerFax");
            DropColumn("dbo.Assets", "ProposedBuyerCellPhone");
            DropColumn("dbo.Assets", "ProposedBuyerWorkPhone");
            DropColumn("dbo.Assets", "ProposedNewBuyerEmail");
        }
    }
}
