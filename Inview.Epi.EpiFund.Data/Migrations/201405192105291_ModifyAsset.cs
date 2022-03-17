namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyAsset : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetCommissions", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetCommissions", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.AssetSalePayments", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetSalePayments", "User_UserId", "dbo.Users");
            DropIndex("dbo.AssetCommissions", new[] { "AssetId" });
            DropIndex("dbo.AssetCommissions", new[] { "User_UserId" });
            DropIndex("dbo.AssetSalePayments", new[] { "AssetId" });
            DropIndex("dbo.AssetSalePayments", new[] { "User_UserId" });
            AddColumn("dbo.Assets", "IsPaper", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Assets", "OperatingStatus", c => c.Int(nullable: false));
            DropTable("dbo.AssetCommissions");
            DropTable("dbo.AssetSalePayments");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.AssetSalePaymentId);
            
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
                .PrimaryKey(t => t.AssetCommissionId);
            
            AlterColumn("dbo.Assets", "OperatingStatus", c => c.String());
            DropColumn("dbo.Assets", "IsPaper");
            CreateIndex("dbo.AssetSalePayments", "User_UserId");
            CreateIndex("dbo.AssetSalePayments", "AssetId");
            CreateIndex("dbo.AssetCommissions", "User_UserId");
            CreateIndex("dbo.AssetCommissions", "AssetId");
            AddForeignKey("dbo.AssetSalePayments", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.AssetSalePayments", "AssetId", "dbo.Assets", "AssetId");
            AddForeignKey("dbo.AssetCommissions", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.AssetCommissions", "AssetId", "dbo.Assets", "AssetId");
        }
    }
}
