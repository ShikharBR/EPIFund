namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtractBedCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ListingStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "AskingPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.Assets", "BathCount", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "BedCount", c => c.Int(nullable: false));
            DropColumn("dbo.Assets", "Sold");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "Sold", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "BedCount", c => c.Int());
            AlterColumn("dbo.Assets", "BathCount", c => c.Int());
            AlterColumn("dbo.Assets", "AskingPrice", c => c.Int(nullable: false));
            DropColumn("dbo.Assets", "ListingStatus");
        }
    }
}
