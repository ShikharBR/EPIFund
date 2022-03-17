namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSellerBoolToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "SubmittedBySeller", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "SubmittedBySeller");
        }
    }
}
