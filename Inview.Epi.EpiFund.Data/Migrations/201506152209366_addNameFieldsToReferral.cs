namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNameFieldsToReferral : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReferrals", "FirstName", c => c.String());
            AddColumn("dbo.UserReferrals", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReferrals", "LastName");
            DropColumn("dbo.UserReferrals", "FirstName");
        }
    }
}
