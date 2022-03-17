namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserRecordsMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRecords",
                c => new
                    {
                        UserRecordId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        UserRecordType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserRecordId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRecords", "UserId", "dbo.Users");
            DropIndex("dbo.UserRecords", new[] { "UserId" });
            DropTable("dbo.UserRecords");
        }
    }
}
