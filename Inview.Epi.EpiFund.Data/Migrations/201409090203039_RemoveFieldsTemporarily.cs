namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFieldsTemporarily : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RealEstateCommercialAssets", "LotSize", c => c.Int(nullable: false));
            DropColumn("dbo.Assets", "HasPositionMortgage");
            DropColumn("dbo.Assets", "MortgageLienType");
            DropColumn("dbo.Assets", "MortgageLienAssumable");
            DropColumn("dbo.RealEstateCommercialAssets", "HasPositionMortgage");
            DropColumn("dbo.RealEstateCommercialAssets", "MortgageLienType");
            DropColumn("dbo.RealEstateCommercialAssets", "MortgageLienAssumable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RealEstateCommercialAssets", "MortgageLienAssumable", c => c.Boolean(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "MortgageLienType", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "HasPositionMortgage", c => c.Boolean());
            AddColumn("dbo.Assets", "MortgageLienAssumable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "MortgageLienType", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "HasPositionMortgage", c => c.Boolean());
            DropColumn("dbo.RealEstateCommercialAssets", "LotSize");
        }
    }
}
