namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTypeToDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetDocuments", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetDocuments", "Type");
        }
    }
}
