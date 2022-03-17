namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSelectedFieldToAssetNAR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetNARMembers", "SelectedNARMemberId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetNARMembers", "SelectedNARMemberId");
        }
    }
}
