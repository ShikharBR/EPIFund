namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDocusignEnvelopeFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocusignEnvelopes", "ReceivedSignedDocument", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocusignEnvelopes", "DateReceived", c => c.DateTime());
            AddColumn("dbo.DocusignEnvelopes", "DocumentType", c => c.Int(nullable: false));
            AddColumn("dbo.DocusignEnvelopes", "AssetNumber", c => c.Int());
            AlterColumn("dbo.DocusignEnvelopes", "EnvelopeId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DocusignEnvelopes", "EnvelopeId", c => c.Guid(nullable: false));
            DropColumn("dbo.DocusignEnvelopes", "AssetNumber");
            DropColumn("dbo.DocusignEnvelopes", "DocumentType");
            DropColumn("dbo.DocusignEnvelopes", "DateReceived");
            DropColumn("dbo.DocusignEnvelopes", "ReceivedSignedDocument");
        }
    }
}
