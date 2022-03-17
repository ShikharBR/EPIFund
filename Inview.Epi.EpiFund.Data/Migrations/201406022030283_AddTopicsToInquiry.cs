namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTopicsToInquiry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inquiries", "Topics", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inquiries", "Topics");
        }
    }
}
