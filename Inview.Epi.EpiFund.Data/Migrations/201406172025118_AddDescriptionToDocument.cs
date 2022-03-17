namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetDocuments", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetDocuments", "Description");
        }
    }
}
