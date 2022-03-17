namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assetchanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ForeclosureInfoes", "RecordNumber", c => c.String());
            AlterColumn("dbo.ForeclosureInfoes", "OriginalMortgageAmount", c => c.Double());
            AlterColumn("dbo.ForeclosureInfoes", "OriginalMortageDate", c => c.DateTime());
            AlterColumn("dbo.ForeclosureInfoes", "SaleDate", c => c.DateTime());
            AlterColumn("dbo.ForeclosureInfoes", "RecordDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForeclosureInfoes", "RecordDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ForeclosureInfoes", "SaleDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ForeclosureInfoes", "OriginalMortageDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ForeclosureInfoes", "OriginalMortgageAmount", c => c.Int(nullable: false));
            AlterColumn("dbo.ForeclosureInfoes", "RecordNumber", c => c.Int(nullable: false));
        }
    }
}
