namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowForMultipleAssetNumbersInDocusignEnvelope : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "OptOutAutoEmails", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocusignEnvelopes", "AssetNumbers", c => c.String());
            DropColumn("dbo.DocusignEnvelopes", "AssetNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocusignEnvelopes", "AssetNumber", c => c.Int());
            DropColumn("dbo.DocusignEnvelopes", "AssetNumbers");
            DropColumn("dbo.Users", "OptOutAutoEmails");
        }
    }
}
