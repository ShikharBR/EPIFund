namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrincipalInterestFieldsToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CommercialInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "MultiFamilyInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "SecuredPaperInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "WholesaleInterest", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "WholesaleInterest");
            DropColumn("dbo.Users", "SecuredPaperInterest");
            DropColumn("dbo.Users", "MultiFamilyInterest");
            DropColumn("dbo.Users", "CommercialInterest");
        }
    }
}
