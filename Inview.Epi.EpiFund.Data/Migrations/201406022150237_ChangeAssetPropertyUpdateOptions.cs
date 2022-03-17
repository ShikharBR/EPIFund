namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAssetPropertyUpdateOptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CommercialRetailInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "CommercialOtherInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "CommercialOfficeInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "MHPInterest", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "CommercialInterest");
            DropColumn("dbo.Users", "WholesaleInterest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "WholesaleInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "CommercialInterest", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "MHPInterest");
            DropColumn("dbo.Users", "CommercialOfficeInterest");
            DropColumn("dbo.Users", "CommercialOtherInterest");
            DropColumn("dbo.Users", "CommercialRetailInterest");
        }
    }
}
