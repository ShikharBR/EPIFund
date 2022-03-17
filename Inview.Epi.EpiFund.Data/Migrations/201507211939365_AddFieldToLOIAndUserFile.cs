namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldToLOIAndUserFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserFiles", "AssetId", c => c.Guid(nullable: false));
            AddColumn("dbo.LOIs", "ReadByListedByUser", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LOIs", "ReadByListedByUser");
            DropColumn("dbo.UserFiles", "AssetId");
        }
    }
}
