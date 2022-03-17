namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeREFieldsToDoubles : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RealEstateCommercialAssets", "CurrentVacancyFactor", c => c.Double());
            AlterColumn("dbo.RealEstateCommercialAssets", "CurrentCalendarYearToDateCashFlow", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RealEstateCommercialAssets", "CurrentCalendarYearToDateCashFlow", c => c.String());
            AlterColumn("dbo.RealEstateCommercialAssets", "CurrentVacancyFactor", c => c.String());
        }
    }
}
