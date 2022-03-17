namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddICFieldsToUser1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LicenseDesc", c => c.String());
            AddColumn("dbo.Users", "LicenseNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LicenseNumber");
            DropColumn("dbo.Users", "LicenseDesc");
        }
    }
}
