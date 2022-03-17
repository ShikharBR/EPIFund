namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsMajorTenantAAARated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "IsMajorTenantAAARated", c => c.Boolean());
            AddColumn("dbo.PaperCommercialAssets", "IsMajorTenantAAARated", c => c.Boolean(nullable: false));
            AddColumn("dbo.RealEstateCommercialAssets", "IsMajorTenantAAARated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RealEstateCommercialAssets", "IsMajorTenantAAARated");
            DropColumn("dbo.PaperCommercialAssets", "IsMajorTenantAAARated");
            DropColumn("dbo.Assets", "IsMajorTenantAAARated");
        }
    }
}
