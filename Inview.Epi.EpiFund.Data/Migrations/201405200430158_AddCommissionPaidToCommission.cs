namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommissionPaidToCommission : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetCommissions", "CommissionPaid", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetCommissions", "CommissionPaid");
        }
    }
}
