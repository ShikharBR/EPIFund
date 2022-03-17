namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAPTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetCommissions",
                c => new
                    {
                        AssetCommissionId = c.Int(nullable: false, identity: true),
                        AssetId = c.Guid(nullable: false),
                        RecordedByUserId = c.Int(nullable: false),
                        ComissionPaymentType = c.Int(nullable: false),
                        CommissionInformation = c.String(),
                        CommissionPaidDate = c.DateTime(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.AssetCommissionId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.AssetId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.AssetSalePayments",
                c => new
                    {
                        AssetSalePaymentId = c.Int(nullable: false, identity: true),
                        AssetId = c.Guid(nullable: false),
                        SaleAmount = c.Double(nullable: false),
                        PaymentInformation = c.String(),
                        PaymentType = c.Int(nullable: false),
                        RecordedByUserId = c.Int(nullable: false),
                        PaymentReceivedDate = c.DateTime(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.AssetSalePaymentId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.AssetId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetSalePayments", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.AssetSalePayments", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetCommissions", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.AssetCommissions", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetSalePayments", new[] { "User_UserId" });
            DropIndex("dbo.AssetSalePayments", new[] { "AssetId" });
            DropIndex("dbo.AssetCommissions", new[] { "User_UserId" });
            DropIndex("dbo.AssetCommissions", new[] { "AssetId" });
            DropTable("dbo.AssetSalePayments");
            DropTable("dbo.AssetCommissions");
        }
    }
}
