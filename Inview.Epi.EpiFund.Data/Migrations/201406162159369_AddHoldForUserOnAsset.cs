namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHoldForUserOnAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "HoldForUserId", c => c.Int());
            AddColumn("dbo.Assets", "HoldStartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "HoldStartDate");
            DropColumn("dbo.Assets", "HoldForUserId");
        }
    }
}
