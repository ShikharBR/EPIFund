namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIdentityOnAssetNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets", "AssetNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assets", "AssetNumber", c => c.Int(nullable: false, identity: true));
        }
    }
}
