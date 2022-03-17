namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class minorTweakToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "BuildingsCount", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "AssetNumber", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assets", "AssetNumber", c => c.Int(nullable: false));
            DropColumn("dbo.Assets", "BuildingsCount");
        }
    }
}
