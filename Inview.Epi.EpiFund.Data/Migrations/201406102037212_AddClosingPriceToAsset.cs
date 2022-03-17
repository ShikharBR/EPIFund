namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClosingPriceToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ClosingPrice", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "ClosingPrice");
        }
    }
}
