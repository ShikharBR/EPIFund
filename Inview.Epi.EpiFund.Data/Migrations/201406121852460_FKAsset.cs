namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKAsset : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "Paper_PaperAssetId", "dbo.PaperAssets");
            DropIndex("dbo.Assets", new[] { "Paper_PaperAssetId" });
            AddColumn("dbo.Assets", "PaperType", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "PaperPropertyType", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "PaperServicingAgent", c => c.String());
            AddColumn("dbo.Assets", "PaperAssignor", c => c.String());
            AddColumn("dbo.Assets", "PaperPrincipalBalance", c => c.Int());
            AddColumn("dbo.Assets", "PaperAskingPrice", c => c.Int());
            AddColumn("dbo.Assets", "PaperApyForAskingPrice", c => c.Single());
            AddColumn("dbo.Assets", "PaperMonthlyInterestIncome", c => c.Double());
            AddColumn("dbo.Assets", "PaperEquityMargin", c => c.Int());
            AddColumn("dbo.Assets", "PaperMonthsInArrears", c => c.Int());
            AddColumn("dbo.Assets", "PaperMaturityDate", c => c.DateTime());
            AddColumn("dbo.Assets", "PaperNextDueDate", c => c.DateTime());
            AddColumn("dbo.Assets", "PaperOriginationDate", c => c.DateTime());
            AddColumn("dbo.Assets", "PriorityMortgageBalance", c => c.Int());
            AddColumn("dbo.Assets", "PaperOriginalInstrDocument", c => c.String());
            AddColumn("dbo.Assets", "PaperCurrent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "PaperNoteRate", c => c.Single());
            AddColumn("dbo.Assets", "PaperInvestmentYield", c => c.Single());
            AddColumn("dbo.Assets", "PaperPriority", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "PaperLtv", c => c.Single());
            AddColumn("dbo.Assets", "PaperCltv", c => c.Single());
            AddColumn("dbo.Assets", "PaperSuccessor", c => c.String());
            AddColumn("dbo.Assets", "PaperSuccessorAddress", c => c.String());
            AddColumn("dbo.Assets", "PaperSuccessorRecordedDocNumber", c => c.String());
            AddColumn("dbo.Assets", "PaperSuccessorDocDate", c => c.String());
            AddColumn("dbo.Assets", "PaperSuccessorType", c => c.String());
            AddColumn("dbo.Assets", "PaperTrustor", c => c.String());
            AddColumn("dbo.Assets", "PaperTrustee", c => c.String());
            DropColumn("dbo.Assets", "Paper_PaperAssetId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "Paper_PaperAssetId", c => c.Guid());
            DropColumn("dbo.Assets", "PaperTrustee");
            DropColumn("dbo.Assets", "PaperTrustor");
            DropColumn("dbo.Assets", "PaperSuccessorType");
            DropColumn("dbo.Assets", "PaperSuccessorDocDate");
            DropColumn("dbo.Assets", "PaperSuccessorRecordedDocNumber");
            DropColumn("dbo.Assets", "PaperSuccessorAddress");
            DropColumn("dbo.Assets", "PaperSuccessor");
            DropColumn("dbo.Assets", "PaperCltv");
            DropColumn("dbo.Assets", "PaperLtv");
            DropColumn("dbo.Assets", "PaperPriority");
            DropColumn("dbo.Assets", "PaperInvestmentYield");
            DropColumn("dbo.Assets", "PaperNoteRate");
            DropColumn("dbo.Assets", "PaperCurrent");
            DropColumn("dbo.Assets", "PaperOriginalInstrDocument");
            DropColumn("dbo.Assets", "PriorityMortgageBalance");
            DropColumn("dbo.Assets", "PaperOriginationDate");
            DropColumn("dbo.Assets", "PaperNextDueDate");
            DropColumn("dbo.Assets", "PaperMaturityDate");
            DropColumn("dbo.Assets", "PaperMonthsInArrears");
            DropColumn("dbo.Assets", "PaperEquityMargin");
            DropColumn("dbo.Assets", "PaperMonthlyInterestIncome");
            DropColumn("dbo.Assets", "PaperApyForAskingPrice");
            DropColumn("dbo.Assets", "PaperAskingPrice");
            DropColumn("dbo.Assets", "PaperPrincipalBalance");
            DropColumn("dbo.Assets", "PaperAssignor");
            DropColumn("dbo.Assets", "PaperServicingAgent");
            DropColumn("dbo.Assets", "PaperPropertyType");
            DropColumn("dbo.Assets", "PaperType");
            CreateIndex("dbo.Assets", "Paper_PaperAssetId");
            AddForeignKey("dbo.Assets", "Paper_PaperAssetId", "dbo.PaperAssets", "PaperAssetId");
        }
    }
}
