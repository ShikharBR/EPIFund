namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToAssetSearch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetSearchCriterias", "WebsiteURLVestingCorporateEntity", c => c.String());
            AddColumn("dbo.AssetSearchCriterias", "NameOfOtherCorporateOfficer2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetSearchCriterias", "NameOfOtherCorporateOfficer2");
            DropColumn("dbo.AssetSearchCriterias", "WebsiteURLVestingCorporateEntity");
        }
    }
}
