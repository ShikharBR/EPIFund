namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addViewableToDocuments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetDocuments", "Viewable", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetDocuments", "Viewable");
        }
    }
}
