namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateEnteredToSearch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetSearchCriterias", "DateEntered", c => c.DateTime(nullable: false));
            AddColumn("dbo.AssetSearchCriterias", "LastUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetSearchCriterias", "LastUpdated");
            DropColumn("dbo.AssetSearchCriterias", "DateEntered");
        }
    }
}
