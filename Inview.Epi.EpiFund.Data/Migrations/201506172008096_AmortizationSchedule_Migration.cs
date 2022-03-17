namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmortizationSchedule_Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "AmortizationSchedule", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "AmortizationSchedule");
        }
    }
}
