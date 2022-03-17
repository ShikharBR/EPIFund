namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCoveredSpacesToAssets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "CoveredParkingSpaces", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "CoveredParkingSpaces");
        }
    }
}
