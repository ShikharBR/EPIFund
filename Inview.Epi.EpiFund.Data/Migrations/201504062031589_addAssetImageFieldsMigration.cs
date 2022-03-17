namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAssetImageFieldsMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetImages", "OriginalFileName", c => c.String());
            AddColumn("dbo.AssetImages", "Size", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetImages", "Size");
            DropColumn("dbo.AssetImages", "OriginalFileName");
        }
    }
}
