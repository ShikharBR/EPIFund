namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMissingPCFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "PCInsuranceOrderedByUserId", c => c.Int());
            AddColumn("dbo.Assets", "PCInsuranceOrderDate", c => c.DateTime());
            AddColumn("dbo.Assets", "PCInsuranceDateOfOrderSubmit", c => c.DateTime());
            AddColumn("dbo.PCInsuranceCompanies", "CompanyPhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PCInsuranceCompanies", "CompanyPhoneNumber");
            DropColumn("dbo.Assets", "PCInsuranceDateOfOrderSubmit");
            DropColumn("dbo.Assets", "PCInsuranceOrderDate");
            DropColumn("dbo.Assets", "PCInsuranceOrderedByUserId");
        }
    }
}
