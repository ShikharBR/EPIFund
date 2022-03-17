namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSampleAssetToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "IsSampleAsset", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "IsSampleAsset");
        }
    }
}
