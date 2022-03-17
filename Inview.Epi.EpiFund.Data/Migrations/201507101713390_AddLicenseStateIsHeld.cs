namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLicenseStateIsHeld : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LicenseStateIsHeld", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LicenseStateIsHeld");
        }
    }
}
