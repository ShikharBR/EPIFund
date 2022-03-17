namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddICAdminContractPayout : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ICAdminContractFeePayouts",
                c => new
                    {
                        ICAdminContractFeePayoutId = c.Int(nullable: false, identity: true),
                        RecordedByUserId = c.Int(nullable: false),
                        DatePaid = c.DateTime(nullable: false),
                        FeeAmount = c.Double(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ICAdminContractFeePayoutId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ICAdminContractFeePayouts", "UserId", "dbo.Users");
            DropIndex("dbo.ICAdminContractFeePayouts", new[] { "UserId" });
            DropTable("dbo.ICAdminContractFeePayouts");
        }
    }
}
