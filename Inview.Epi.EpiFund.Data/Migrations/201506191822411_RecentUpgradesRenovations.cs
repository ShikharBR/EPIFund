namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecentUpgradesRenovations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "RecentUpgradesRenovations", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "RecentUpgradesRenovations");
        }
    }
}
