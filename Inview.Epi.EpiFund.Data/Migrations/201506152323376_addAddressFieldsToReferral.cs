namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAddressFieldsToReferral : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReferrals", "City", c => c.String());
            AddColumn("dbo.UserReferrals", "State", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReferrals", "State");
            DropColumn("dbo.UserReferrals", "City");
        }
    }
}
