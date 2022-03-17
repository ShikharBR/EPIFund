namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRealEstateResidential : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RealEstateResidentialAssets", "Language", c => c.String());
            AlterColumn("dbo.RealEstateResidentialAssets", "Bedrooms", c => c.String());
            AlterColumn("dbo.RealEstateResidentialAssets", "Bathrooms", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RealEstateResidentialAssets", "Bathrooms", c => c.Int());
            AlterColumn("dbo.RealEstateResidentialAssets", "Bedrooms", c => c.Int());
            AlterColumn("dbo.RealEstateResidentialAssets", "Language", c => c.Int());
        }
    }
}
