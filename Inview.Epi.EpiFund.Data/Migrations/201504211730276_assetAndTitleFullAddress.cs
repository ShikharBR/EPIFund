namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assetAndTitleFullAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "CorporateOwnershipAddress2", c => c.String());
            AddColumn("dbo.Assets", "CorporateOwnershipCity", c => c.String());
            AddColumn("dbo.Assets", "CorporateOwnershipState", c => c.String());
            AddColumn("dbo.Assets", "CorporateOwnershipZip", c => c.String());
            AddColumn("dbo.TitleCompanies", "TitleCompAddress2", c => c.String());
            AddColumn("dbo.TitleCompanies", "City", c => c.String());
            AddColumn("dbo.TitleCompanies", "State", c => c.String());
            AddColumn("dbo.TitleCompanies", "Zip", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TitleCompanies", "Zip");
            DropColumn("dbo.TitleCompanies", "State");
            DropColumn("dbo.TitleCompanies", "City");
            DropColumn("dbo.TitleCompanies", "TitleCompAddress2");
            DropColumn("dbo.Assets", "CorporateOwnershipZip");
            DropColumn("dbo.Assets", "CorporateOwnershipState");
            DropColumn("dbo.Assets", "CorporateOwnershipCity");
            DropColumn("dbo.Assets", "CorporateOwnershipAddress2");
        }
    }
}
