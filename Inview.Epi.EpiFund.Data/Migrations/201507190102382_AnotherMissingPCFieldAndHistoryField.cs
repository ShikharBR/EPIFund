namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnotherMissingPCFieldAndHistoryField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "PCInsuranceCompanyUserId", c => c.Int());
            AddColumn("dbo.AssetOrderHistories", "HistoryType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetOrderHistories", "HistoryType");
            DropColumn("dbo.Assets", "PCInsuranceCompanyUserId");
        }
    }
}
