namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAdminOnlyFieldsToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "DateOfSale", c => c.DateTime());
            AddColumn("dbo.Assets", "SalesPrice", c => c.Double());
            AddColumn("dbo.Assets", "CalculatedPPU", c => c.Double());
            AddColumn("dbo.Assets", "CalculatedPPSqFt", c => c.Double());
            AddColumn("dbo.Assets", "BuyerName", c => c.String());
            AddColumn("dbo.Assets", "BuyerAddress", c => c.String());
            AddColumn("dbo.Assets", "Terms", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "Terms");
            DropColumn("dbo.Assets", "BuyerAddress");
            DropColumn("dbo.Assets", "BuyerName");
            DropColumn("dbo.Assets", "CalculatedPPSqFt");
            DropColumn("dbo.Assets", "CalculatedPPU");
            DropColumn("dbo.Assets", "SalesPrice");
            DropColumn("dbo.Assets", "DateOfSale");
        }
    }
}
