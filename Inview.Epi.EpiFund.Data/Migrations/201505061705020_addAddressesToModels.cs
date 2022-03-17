namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAddressesToModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "PropertyAddress2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "PropertyAddress2");
        }
    }
}
