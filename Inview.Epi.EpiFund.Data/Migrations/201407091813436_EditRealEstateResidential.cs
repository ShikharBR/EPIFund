namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditRealEstateResidential : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RealEstateResidentialAssets", "PropertyInHOA", c => c.Boolean());
            AlterColumn("dbo.RealEstateResidentialAssets", "HOALiensOnProperty", c => c.Boolean());
            AlterColumn("dbo.RealEstateResidentialAssets", "DateOfBirth", c => c.DateTime());
            AlterColumn("dbo.RealEstateResidentialAssets", "FirstMCCurrentPrincipalBalance", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RealEstateResidentialAssets", "FirstMCCurrentPrincipalBalance", c => c.Double(nullable: false));
            AlterColumn("dbo.RealEstateResidentialAssets", "DateOfBirth", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RealEstateResidentialAssets", "HOALiensOnProperty", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RealEstateResidentialAssets", "PropertyInHOA", c => c.Boolean(nullable: false));
        }
    }
}
