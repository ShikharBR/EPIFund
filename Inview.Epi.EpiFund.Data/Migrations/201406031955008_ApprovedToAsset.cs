namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApprovedToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "Approved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "Approved");
        }
    }
}
