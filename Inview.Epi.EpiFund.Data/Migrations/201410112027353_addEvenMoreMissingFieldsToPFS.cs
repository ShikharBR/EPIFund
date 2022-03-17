namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEvenMoreMissingFieldsToPFS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonalFinancialStatements", "NetInvestmentIncome", c => c.String());
            AddColumn("dbo.PersonalFinancialStatements", "RealEstateOwnedOriginalCostA", c => c.String());
            AddColumn("dbo.PersonalFinancialStatements", "RealEstateOwnedOriginalCostB", c => c.String());
            AddColumn("dbo.PersonalFinancialStatements", "RealEstateOwnedOriginalCostC", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonalFinancialStatements", "RealEstateOwnedOriginalCostC");
            DropColumn("dbo.PersonalFinancialStatements", "RealEstateOwnedOriginalCostB");
            DropColumn("dbo.PersonalFinancialStatements", "RealEstateOwnedOriginalCostA");
            DropColumn("dbo.PersonalFinancialStatements", "NetInvestmentIncome");
        }
    }
}
