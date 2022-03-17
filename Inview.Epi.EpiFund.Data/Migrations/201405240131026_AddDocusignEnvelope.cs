namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocusignEnvelope : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocusignEnvelopes",
                c => new
                    {
                        DocusignEnvelopeId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        EnvelopeId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.DocusignEnvelopeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DocusignEnvelopes");
        }
    }
}
