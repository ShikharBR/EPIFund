namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOptionalPropertyRequiresMajortenant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SearchCriteriaDemographicDetails", "PropertyRequiresMajorTenantOptional", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SearchCriteriaDemographicDetails", "PropertyRequiresMajorTenantOptional");
        }
    }
}
