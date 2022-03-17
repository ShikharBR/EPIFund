namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssetLock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetLocks",
                c => new
                    {
                        AssetLockId = c.Int(nullable: false, identity: true),
                        AssetId = c.Guid(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssetLockId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AssetLocks");
        }
    }
}
