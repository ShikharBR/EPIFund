namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOFSandWebEmailToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "OfficerOfSeller", c => c.String());
            AddColumn("dbo.Assets", "WebsiteEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "WebsiteEmail");
            DropColumn("dbo.Assets", "OfficerOfSeller");
        }
    }
}
