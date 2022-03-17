namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorporateOwnershipFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "CorporateOwnershipAddress", c => c.String());
            AddColumn("dbo.Assets", "CorporateOwnershipOfficer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "CorporateOwnershipOfficer");
            DropColumn("dbo.Assets", "CorporateOwnershipAddress");
        }
    }
}
