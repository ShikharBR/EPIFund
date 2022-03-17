namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPCInsuranceStructure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PCInsuranceCompanies",
                c => new
                    {
                        PCInsuranceCompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        CompanyURL = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CompanyAddress1 = c.String(),
                        CompanyAddress2 = c.String(),
                        CompanyCity = c.String(),
                        CompanyState = c.String(),
                        CompanyZip = c.String(),
                    })
                .PrimaryKey(t => t.PCInsuranceCompanyId);
            
            CreateTable(
                "dbo.PCInsuranceCompanyManagers",
                c => new
                    {
                        PCInsuranceCompanyManagerId = c.Int(nullable: false, identity: true),
                        PCInsuranceCompanyId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PCInsuranceCompanyManagerId)
                .ForeignKey("dbo.PCInsuranceCompanies", t => t.PCInsuranceCompanyId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.PCInsuranceCompanyId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Assets", "PCInsuranceCompanyId", c => c.Int());
            AddColumn("dbo.Assets", "PCInsuranceOrderStatus", c => c.Int(nullable: false));
            AddColumn("dbo.UserRecords", "Data", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PCInsuranceCompanyManagers", "UserId", "dbo.Users");
            DropForeignKey("dbo.PCInsuranceCompanyManagers", "PCInsuranceCompanyId", "dbo.PCInsuranceCompanies");
            DropIndex("dbo.PCInsuranceCompanyManagers", new[] { "UserId" });
            DropIndex("dbo.PCInsuranceCompanyManagers", new[] { "PCInsuranceCompanyId" });
            DropColumn("dbo.UserRecords", "Data");
            DropColumn("dbo.Assets", "PCInsuranceOrderStatus");
            DropColumn("dbo.Assets", "PCInsuranceCompanyId");
            DropTable("dbo.PCInsuranceCompanyManagers");
            DropTable("dbo.PCInsuranceCompanies");
        }
    }
}
