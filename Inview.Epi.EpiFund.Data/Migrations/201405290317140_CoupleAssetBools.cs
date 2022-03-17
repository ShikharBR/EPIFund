namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoupleAssetBools : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "HasHOA", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "HasIncome", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "HasIncome");
            DropColumn("dbo.Assets", "HasHOA");
        }
    }
}
