namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addContractFeePayoutTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContractFeePayouts",
                c => new
                    {
                        ContractFeePayoutId = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        RecordedByUserId = c.Int(nullable: false),
                        DatePaid = c.DateTime(nullable: false),
                        FeeAmount = c.Double(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContractFeePayoutId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContractFeePayouts", "UserId", "dbo.Users");
            DropIndex("dbo.ContractFeePayouts", new[] { "UserId" });
            DropTable("dbo.ContractFeePayouts");
        }
    }
}
