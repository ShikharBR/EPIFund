namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBoolToListingAgents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetListingAgents", "NotOnList", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetListingAgents", "NotOnList");
        }
    }
}
