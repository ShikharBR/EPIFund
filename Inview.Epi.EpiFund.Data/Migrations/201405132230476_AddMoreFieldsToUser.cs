namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreFieldsToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CompanyName", c => c.String());
            AddColumn("dbo.Users", "StateOfOriginCorporateEntity", c => c.String());
            AddColumn("dbo.Users", "CorporateEntityType", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "IsCertificateOfGoodStandingAvailable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsCertificateOfGoodStandingAvailable");
            DropColumn("dbo.Users", "CorporateEntityType");
            DropColumn("dbo.Users", "StateOfOriginCorporateEntity");
            DropColumn("dbo.Users", "CompanyName");
        }
    }
}
