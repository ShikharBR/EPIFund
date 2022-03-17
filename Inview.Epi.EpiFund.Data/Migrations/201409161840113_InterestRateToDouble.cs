namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterestRateToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets", "InterestRate", c => c.Double());
            AlterColumn("dbo.RealEstateCommercialAssets", "InterestRate", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RealEstateCommercialAssets", "InterestRate", c => c.Int());
            AlterColumn("dbo.Assets", "InterestRate", c => c.Int());
        }
    }
}
