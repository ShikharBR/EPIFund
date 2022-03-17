namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertTypeIntoDemographicDetailsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SearchCriteriaDemographicDetails", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SearchCriteriaDemographicDetails", "Type");
        }
    }
}
