namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeImageAndDocStruct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetDocuments", "URL", c => c.String());
            AddColumn("dbo.AssetDocuments", "FileName", c => c.String());
            AddColumn("dbo.AssetDocuments", "ContentType", c => c.String());
            AddColumn("dbo.AssetImages", "Url", c => c.String());
            AddColumn("dbo.AssetImages", "FileName", c => c.String());
            AddColumn("dbo.AssetImages", "ContentType", c => c.String());
            DropColumn("dbo.AssetDocuments", "Location");
            DropColumn("dbo.AssetImages", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssetImages", "Location", c => c.String());
            AddColumn("dbo.AssetDocuments", "Location", c => c.String());
            DropColumn("dbo.AssetImages", "ContentType");
            DropColumn("dbo.AssetImages", "FileName");
            DropColumn("dbo.AssetImages", "Url");
            DropColumn("dbo.AssetDocuments", "ContentType");
            DropColumn("dbo.AssetDocuments", "FileName");
            DropColumn("dbo.AssetDocuments", "URL");
        }
    }
}
