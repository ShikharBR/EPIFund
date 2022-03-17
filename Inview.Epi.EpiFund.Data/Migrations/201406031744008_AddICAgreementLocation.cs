namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddICAgreementLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ICFileLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ICFileLocation");
        }
    }
}
