namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProformaColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ProformaAnnualIncome", c => c.Double(nullable: false));
            AddColumn("dbo.Assets", "ProformaMonthlyIncome", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "ProformaMonthlyIncome");
            DropColumn("dbo.Assets", "ProformaAnnualIncome");
        }
    }
}
