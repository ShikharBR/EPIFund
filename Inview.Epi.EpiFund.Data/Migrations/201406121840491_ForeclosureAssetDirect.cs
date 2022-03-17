namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeclosureAssetDirect : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "Foreclosure_ForeclosureInfoId", "dbo.ForeclosureInfoes");
            DropForeignKey("dbo.Assets", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets");
            DropIndex("dbo.Assets", new[] { "Foreclosure_ForeclosureInfoId" });
            DropIndex("dbo.Assets", new[] { "User_UserId" });
            DropIndex("dbo.AssetDocuments", new[] { "Asset_AssetId" });
            RenameColumn(table: "dbo.AssetDocuments", name: "Asset_AssetId", newName: "AssetId");
            AddColumn("dbo.Assets", "ForeclosureLender", c => c.String());
            AddColumn("dbo.Assets", "ForeclosurePosition", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "ForeclosureRecordNumber", c => c.String());
            AddColumn("dbo.Assets", "ForeclosureOriginalMortgageAmount", c => c.Double());
            AddColumn("dbo.Assets", "ForeclosureOriginalMortageDate", c => c.DateTime());
            AddColumn("dbo.Assets", "ForeclosureSaleDate", c => c.DateTime());
            AddColumn("dbo.Assets", "ForeclosureRecordDate", c => c.DateTime());
            AddColumn("dbo.AssetImages", "AssetId", c => c.Guid(nullable: false));
            AddColumn("dbo.AssetImages", "Asset_AssetId1", c => c.Guid());
            AlterColumn("dbo.Assets", "ListedByUserId", c => c.Int());
            AlterColumn("dbo.AssetDocuments", "AssetId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AssetDocuments", "AssetId");
            CreateIndex("dbo.AssetImages", "Asset_AssetId1");
            AddForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetImages", "Asset_AssetId1", "dbo.Assets", "AssetId");
            DropColumn("dbo.Assets", "Foreclosure_ForeclosureInfoId");
            DropColumn("dbo.Assets", "User_UserId");
            DropTable("dbo.ForeclosureInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ForeclosureInfoes",
                c => new
                    {
                        ForeclosureInfoId = c.Guid(nullable: false),
                        Lender = c.String(),
                        Position = c.Int(nullable: false),
                        RecordNumber = c.String(),
                        OriginalMortgageAmount = c.Double(),
                        OriginalMortageDate = c.DateTime(),
                        SaleDate = c.DateTime(),
                        RecordDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ForeclosureInfoId);
            
            AddColumn("dbo.Assets", "User_UserId", c => c.Int());
            AddColumn("dbo.Assets", "Foreclosure_ForeclosureInfoId", c => c.Guid());
            DropForeignKey("dbo.AssetImages", "Asset_AssetId1", "dbo.Assets");
            DropForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets");
            DropIndex("dbo.AssetImages", new[] { "Asset_AssetId1" });
            DropIndex("dbo.AssetDocuments", new[] { "AssetId" });
            AlterColumn("dbo.AssetDocuments", "AssetId", c => c.Guid());
            AlterColumn("dbo.Assets", "ListedByUserId", c => c.Int(nullable: false));
            DropColumn("dbo.AssetImages", "Asset_AssetId1");
            DropColumn("dbo.AssetImages", "AssetId");
            DropColumn("dbo.Assets", "ForeclosureRecordDate");
            DropColumn("dbo.Assets", "ForeclosureSaleDate");
            DropColumn("dbo.Assets", "ForeclosureOriginalMortageDate");
            DropColumn("dbo.Assets", "ForeclosureOriginalMortgageAmount");
            DropColumn("dbo.Assets", "ForeclosureRecordNumber");
            DropColumn("dbo.Assets", "ForeclosurePosition");
            DropColumn("dbo.Assets", "ForeclosureLender");
            RenameColumn(table: "dbo.AssetDocuments", name: "AssetId", newName: "Asset_AssetId");
            CreateIndex("dbo.AssetDocuments", "Asset_AssetId");
            CreateIndex("dbo.Assets", "User_UserId");
            CreateIndex("dbo.Assets", "Foreclosure_ForeclosureInfoId");
            AddForeignKey("dbo.AssetImages", "Asset_AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.Assets", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Assets", "Foreclosure_ForeclosureInfoId", "dbo.ForeclosureInfoes", "ForeclosureInfoId");
        }
    }
}
