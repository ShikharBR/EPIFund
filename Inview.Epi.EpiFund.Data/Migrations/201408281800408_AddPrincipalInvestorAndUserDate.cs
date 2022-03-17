namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrincipalInvestorAndUserDate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrincipalInvestors",
                c => new
                    {
                        PrincipalInvestorId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        CompanyName = c.String(),
                        CompanyAddressLine1 = c.String(),
                        CompanyAddressLine2 = c.String(),
                        CompanyCity = c.String(),
                        CompanyState = c.String(),
                        CompanyZip = c.String(),
                        CellPhoneNumber = c.String(),
                        WorkPhoneNumber = c.String(),
                        FaxNumber = c.String(),
                        ReferredByUserId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PrincipalInvestorId);
            
            AddColumn("dbo.Users", "SignupDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "SignupDate");
            DropTable("dbo.PrincipalInvestors");
        }
    }
}
