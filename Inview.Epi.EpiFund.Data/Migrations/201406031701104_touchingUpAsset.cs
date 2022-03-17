namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class touchingUpAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ListedByRealtor", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "ProformaMiscIncome", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "ProformaVacancyFac", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "CurrentVacancyFac", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "LastReportedOccPercent", c => c.Single(nullable: false));
            AddColumn("dbo.Assets", "ProformaAnnualOperExpenses", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "ProformaAoeFactorAsPerOfSGI", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "ProformaAoeFactorAsPerOfSGI");
            DropColumn("dbo.Assets", "ProformaAnnualOperExpenses");
            DropColumn("dbo.Assets", "LastReportedOccPercent");
            DropColumn("dbo.Assets", "CurrentVacancyFac");
            DropColumn("dbo.Assets", "ProformaVacancyFac");
            DropColumn("dbo.Assets", "ProformaMiscIncome");
            DropColumn("dbo.Assets", "ListedByRealtor");
        }
    }
}
