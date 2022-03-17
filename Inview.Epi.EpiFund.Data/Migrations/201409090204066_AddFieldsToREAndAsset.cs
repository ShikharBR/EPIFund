namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToREAndAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "HasPositionMortgage", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "MortgageLienType", c => c.Int());
            AddColumn("dbo.Assets", "MortgageLienAssumable", c => c.Int());
            AddColumn("dbo.RealEstateCommercialAssets", "HasPositionMortgage", c => c.Int(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "MortgageLienType", c => c.Int());
            AddColumn("dbo.RealEstateCommercialAssets", "MortgageLienAssumable", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RealEstateCommercialAssets", "MortgageLienAssumable");
            DropColumn("dbo.RealEstateCommercialAssets", "MortgageLienType");
            DropColumn("dbo.RealEstateCommercialAssets", "HasPositionMortgage");
            DropColumn("dbo.Assets", "MortgageLienAssumable");
            DropColumn("dbo.Assets", "MortgageLienType");
            DropColumn("dbo.Assets", "HasPositionMortgage");
        }
    }
}
