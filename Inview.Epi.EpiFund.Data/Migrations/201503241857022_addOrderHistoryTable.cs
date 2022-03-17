namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrderHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetOrderHistories",
                c => new
                    {
                        AssetOrderHistoryId = c.Int(nullable: false, identity: true),
                        AssetId = c.Guid(nullable: false),
                        UserId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AssetOrderHistoryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AssetOrderHistories");
        }
    }
}
