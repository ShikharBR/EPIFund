namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddICFieldsToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CorporateTIN", c => c.String());
            AddColumn("dbo.Users", "StateLicenseDesc", c => c.String());
            AddColumn("dbo.Users", "StateLicenseNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "StateLicenseNumber");
            DropColumn("dbo.Users", "StateLicenseDesc");
            DropColumn("dbo.Users", "CorporateTIN");
        }
    }
}
