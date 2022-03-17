namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastReportedOccupancyDateToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "LastReportedOccupancyDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "LastReportedOccupancyDate");
        }
    }
}
