namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetEdits_Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "PropHoldTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "LeaseholdMaturityDate", c => c.DateTime());
            AddColumn("dbo.Assets", "PropLastUpdated", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "PropLastUpdated");
            DropColumn("dbo.Assets", "LeaseholdMaturityDate");
            DropColumn("dbo.Assets", "PropHoldTypeId");
        }
    }
}
