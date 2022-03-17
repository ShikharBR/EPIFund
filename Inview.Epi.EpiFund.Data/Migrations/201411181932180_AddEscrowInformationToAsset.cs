namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEscrowInformationToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "EscrowCompany", c => c.String());
            AddColumn("dbo.Assets", "EscrowCompanyAddress", c => c.String());
            AddColumn("dbo.Assets", "EscrowCompanyPhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "EscrowCompanyPhoneNumber");
            DropColumn("dbo.Assets", "EscrowCompanyAddress");
            DropColumn("dbo.Assets", "EscrowCompany");
        }
    }
}
