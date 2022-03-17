namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CorporateTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CorporateTitle");
        }
    }
}
