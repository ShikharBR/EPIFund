namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingHowDetailsStored : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "PropertyDetailsString", c => c.String());
            AddColumn("dbo.Assets", "MFDetailsString", c => c.String());
            AddColumn("dbo.Assets", "EstDefMaintenanceDetailsString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "EstDefMaintenanceDetailsString");
            DropColumn("dbo.Assets", "MFDetailsString");
            DropColumn("dbo.Assets", "PropertyDetailsString");
        }
    }
}
