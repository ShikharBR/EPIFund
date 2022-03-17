namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeContactTimeToMultiple : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PreferredContactTimes", c => c.String());
            AddColumn("dbo.RealEstateCommercialAssets", "PreferredContactTimes", c => c.String());
            DropColumn("dbo.Users", "PreferredContactTime");
            DropColumn("dbo.RealEstateCommercialAssets", "PreferredContactTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RealEstateCommercialAssets", "PreferredContactTime", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "PreferredContactTime", c => c.Int(nullable: false));
            DropColumn("dbo.RealEstateCommercialAssets", "PreferredContactTimes");
            DropColumn("dbo.Users", "PreferredContactTimes");
        }
    }
}
