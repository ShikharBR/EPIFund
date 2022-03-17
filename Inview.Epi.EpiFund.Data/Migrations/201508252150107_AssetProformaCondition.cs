namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetProformaCondition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "isOfferingMemorandum", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "isOfferingMemorandum");
        }
    }
}
