namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUserReferralTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReferrals", "ReferralCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReferrals", "ReferralCode");
        }
    }
}
