namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDecimalsToStringsForSearchCriterias : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AssetSearchCriterias", "TermsSought", c => c.String());
            AlterColumn("dbo.SearchCriteriaDemographicDetails", "SingleWideSpaceRatioForAllSpaces", c => c.String());
            AlterColumn("dbo.SearchCriteriaDemographicDetails", "DoubleWideSpaceRatioForAllSpaces", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SearchCriteriaDemographicDetails", "DoubleWideSpaceRatioForAllSpaces", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.SearchCriteriaDemographicDetails", "SingleWideSpaceRatioForAllSpaces", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.AssetSearchCriterias", "TermsSought", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
