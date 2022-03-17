namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMbaUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MBAUsers",
                c => new
                    {
                        MBAUserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        CompanyName = c.String(),
                        CompanyWebsite = c.String(),
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
                .PrimaryKey(t => t.MBAUserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MBAUsers");
        }
    }
}
