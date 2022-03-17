namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInquiry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inquiries",
                c => new
                    {
                        InquiryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ContactNumber = c.String(),
                        EmailAddress = c.String(),
                        Comments = c.String(),
                        Responded = c.Boolean(nullable: false),
                        DateOfInquiry = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InquiryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Inquiries");
        }
    }
}
