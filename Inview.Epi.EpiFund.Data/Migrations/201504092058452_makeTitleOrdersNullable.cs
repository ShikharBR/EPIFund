namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeTitleOrdersNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TitleOrderPayments", new[] { "TitleCompanyId" });
            DropIndex("dbo.TitleOrderPayments", new[] { "AssetId" });
            AlterColumn("dbo.TitleOrderPayments", "TitleCompanyId", c => c.Int());
            AlterColumn("dbo.TitleOrderPayments", "PaymentType", c => c.Int());
            AlterColumn("dbo.TitleOrderPayments", "RecordedByUserId", c => c.Int());
            AlterColumn("dbo.TitleOrderPayments", "AssetId", c => c.Guid());
            AlterColumn("dbo.TitleOrderPayments", "PaymentReceivedDate", c => c.DateTime());
            AlterColumn("dbo.TitleOrderPayments", "PaidAmount", c => c.Double());
            CreateIndex("dbo.TitleOrderPayments", "TitleCompanyId");
            CreateIndex("dbo.TitleOrderPayments", "AssetId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TitleOrderPayments", new[] { "AssetId" });
            DropIndex("dbo.TitleOrderPayments", new[] { "TitleCompanyId" });
            AlterColumn("dbo.TitleOrderPayments", "PaidAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.TitleOrderPayments", "PaymentReceivedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TitleOrderPayments", "AssetId", c => c.Guid(nullable: false));
            AlterColumn("dbo.TitleOrderPayments", "RecordedByUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.TitleOrderPayments", "PaymentType", c => c.Int(nullable: false));
            AlterColumn("dbo.TitleOrderPayments", "TitleCompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.TitleOrderPayments", "AssetId");
            CreateIndex("dbo.TitleOrderPayments", "TitleCompanyId");
        }
    }
}
