namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHasIncomeReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "HasIncomeReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "HasIncomeReason");
        }
    }
}
