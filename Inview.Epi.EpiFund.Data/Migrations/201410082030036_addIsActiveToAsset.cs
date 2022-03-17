namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsActiveToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "IsActive");
        }
    }
}
