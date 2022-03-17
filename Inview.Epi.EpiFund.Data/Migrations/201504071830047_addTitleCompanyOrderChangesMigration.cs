namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTitleCompanyOrderChangesMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TitleOrderPayments",
                c => new
                    {
                        TitleOrderPaymentId = c.Int(nullable: false, identity: true),
                        PaymentInformation = c.String(),
                        PaymentType = c.Int(nullable: false),
                        RecordedByUserId = c.Int(nullable: false),
                        AssetId = c.Guid(nullable: false),
                        PaymentReceivedDate = c.DateTime(nullable: false),
                        PaidAmount = c.Double(nullable: false),
                        TitleCompany_TitleCompanyId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TitleOrderPaymentId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .ForeignKey("dbo.TitleCompanies", t => t.TitleCompany_TitleCompanyId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.AssetId)
                .Index(t => t.TitleCompany_TitleCompanyId)
                .Index(t => t.User_UserId);
            
            AddColumn("dbo.Assets", "DateOfOrderSubmit", c => c.DateTime());
            AddColumn("dbo.TitleCompanies", "CurrentRate", c => c.Double());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TitleOrderPayments", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.TitleOrderPayments", "TitleCompany_TitleCompanyId", "dbo.TitleCompanies");
            DropForeignKey("dbo.TitleOrderPayments", "AssetId", "dbo.Assets");
            DropIndex("dbo.TitleOrderPayments", new[] { "User_UserId" });
            DropIndex("dbo.TitleOrderPayments", new[] { "TitleCompany_TitleCompanyId" });
            DropIndex("dbo.TitleOrderPayments", new[] { "AssetId" });
            DropColumn("dbo.TitleCompanies", "CurrentRate");
            DropColumn("dbo.Assets", "DateOfOrderSubmit");
            DropTable("dbo.TitleOrderPayments");
        }
    }
}
