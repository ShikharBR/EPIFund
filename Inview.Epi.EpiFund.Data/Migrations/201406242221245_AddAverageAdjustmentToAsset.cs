namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAverageAdjustmentToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations");
        }
    }
}
