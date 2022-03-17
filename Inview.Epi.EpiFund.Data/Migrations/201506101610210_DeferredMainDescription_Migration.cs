namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeferredMainDescription_Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetDeferredMaintenanceItems", "ItemDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetDeferredMaintenanceItems", "ItemDescription");
        }
    }
}
