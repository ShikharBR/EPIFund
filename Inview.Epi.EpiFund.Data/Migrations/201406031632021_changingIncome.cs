namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingIncome : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "Income_AssetIncomeId", "dbo.AssetIncomes");
            DropIndex("dbo.Assets", new[] { "Income_AssetIncomeId" });
            AddColumn("dbo.Assets", "MonthlyGrossIncome", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "AnnualGrossIncome", c => c.Int(nullable: false));
            DropColumn("dbo.Assets", "Income_AssetIncomeId");
            DropColumn("dbo.AssetIncomes", "MonthlyGrossIncome");
            DropColumn("dbo.AssetIncomes", "AnnualGrossIncome");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssetIncomes", "AnnualGrossIncome", c => c.Int(nullable: false));
            AddColumn("dbo.AssetIncomes", "MonthlyGrossIncome", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "Income_AssetIncomeId", c => c.Guid());
            DropColumn("dbo.Assets", "AnnualGrossIncome");
            DropColumn("dbo.Assets", "MonthlyGrossIncome");
            CreateIndex("dbo.Assets", "Income_AssetIncomeId");
            AddForeignKey("dbo.Assets", "Income_AssetIncomeId", "dbo.AssetIncomes", "AssetIncomeId");
        }
    }
}
