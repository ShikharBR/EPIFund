namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEscrowFieldsToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ProposedCOEDate", c => c.DateTime());
            AddColumn("dbo.Assets", "ActualCOEDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "ActualCOEDate");
            DropColumn("dbo.Assets", "ProposedCOEDate");
        }
    }
}
