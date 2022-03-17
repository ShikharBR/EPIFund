namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetChangesPt2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "FlyerImage_AssetImageId", "dbo.AssetImages");
            DropIndex("dbo.Assets", new[] { "FlyerImage_AssetImageId" });
            AddColumn("dbo.AssetImages", "IsFlyerImage", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Assets", "AnnualPropertyTax", c => c.Double(nullable: false));
            AlterColumn("dbo.Assets", "CurrentBpo", c => c.Double(nullable: false));
            AlterColumn("dbo.Assets", "MonthlyGrossIncome", c => c.Double(nullable: false));
            AlterColumn("dbo.Assets", "AnnualGrossIncome", c => c.Double(nullable: false));
            AlterColumn("dbo.Assets", "ProformaMiscIncome", c => c.Double(nullable: false));
            AlterColumn("dbo.Assets", "ProformaVacancyFac", c => c.Double(nullable: false));
            AlterColumn("dbo.Assets", "CurrentVacancyFac", c => c.Double(nullable: false));
            AlterColumn("dbo.PaperAssets", "MonthlyInterestIncome", c => c.Double(nullable: false));
            AlterColumn("dbo.PaperAssets", "MaturityDate", c => c.DateTime());
            AlterColumn("dbo.PaperAssets", "NextDueDate", c => c.DateTime());
            AlterColumn("dbo.PaperAssets", "OriginationDate", c => c.DateTime());
            DropColumn("dbo.Assets", "FlyerImage_AssetImageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "FlyerImage_AssetImageId", c => c.Guid());
            AlterColumn("dbo.PaperAssets", "OriginationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PaperAssets", "NextDueDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PaperAssets", "MaturityDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PaperAssets", "MonthlyInterestIncome", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "CurrentVacancyFac", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "ProformaVacancyFac", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "ProformaMiscIncome", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "AnnualGrossIncome", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "MonthlyGrossIncome", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "CurrentBpo", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "AnnualPropertyTax", c => c.Int(nullable: false));
            DropColumn("dbo.AssetImages", "IsFlyerImage");
            CreateIndex("dbo.Assets", "FlyerImage_AssetImageId");
            AddForeignKey("dbo.Assets", "FlyerImage_AssetImageId", "dbo.AssetImages", "AssetImageId");
        }
    }
}
