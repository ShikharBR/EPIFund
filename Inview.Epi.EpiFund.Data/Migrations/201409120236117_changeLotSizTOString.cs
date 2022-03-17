namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeLotSizTOString : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RealEstateCommercialAssets", "LotSize");
            AddColumn("dbo.RealEstateCommercialAssets", "LotSize", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RealEstateCommercialAssets", "LotSize");
            AddColumn("dbo.RealEstateCommercialAssets", "LotSize", c => c.Int(nullable: false));
        }
    }
}
