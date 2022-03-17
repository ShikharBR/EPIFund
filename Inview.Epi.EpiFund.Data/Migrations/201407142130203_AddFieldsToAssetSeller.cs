namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToAssetSeller : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetSellers", "NameOfCoPrincipal", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryName", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryFullName", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryContactAddress", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryCity", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryState", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryZip", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryPhoneHome", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryPhoneCell", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryPhoneWork", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryFax", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryEmail", c => c.String());
            AddColumn("dbo.AssetSellers", "BeneficiaryAccountNumber", c => c.String());
            AddColumn("dbo.AssetSellers", "SellerType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetSellers", "SellerType");
            DropColumn("dbo.AssetSellers", "BeneficiaryAccountNumber");
            DropColumn("dbo.AssetSellers", "BeneficiaryEmail");
            DropColumn("dbo.AssetSellers", "BeneficiaryFax");
            DropColumn("dbo.AssetSellers", "BeneficiaryPhoneWork");
            DropColumn("dbo.AssetSellers", "BeneficiaryPhoneCell");
            DropColumn("dbo.AssetSellers", "BeneficiaryPhoneHome");
            DropColumn("dbo.AssetSellers", "BeneficiaryZip");
            DropColumn("dbo.AssetSellers", "BeneficiaryState");
            DropColumn("dbo.AssetSellers", "BeneficiaryCity");
            DropColumn("dbo.AssetSellers", "BeneficiaryContactAddress");
            DropColumn("dbo.AssetSellers", "BeneficiaryFullName");
            DropColumn("dbo.AssetSellers", "BeneficiaryName");
            DropColumn("dbo.AssetSellers", "NameOfCoPrincipal");
        }
    }
}
