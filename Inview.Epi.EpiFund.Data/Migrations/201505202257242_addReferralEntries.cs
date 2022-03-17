namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReferralEntries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserReferrals",
                c => new
                    {
                        UserReferralId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ReferralEmail = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserReferralId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Users", "ReferredByUserId", c => c.Int());
            AddColumn("dbo.Users", "ReferralStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserReferrals", "UserId", "dbo.Users");
            DropIndex("dbo.UserReferrals", new[] { "UserId" });
            DropColumn("dbo.Users", "ReferralStatus");
            DropColumn("dbo.Users", "ReferredByUserId");
            DropTable("dbo.UserReferrals");
        }
    }
}
