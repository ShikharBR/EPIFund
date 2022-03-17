namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedTypos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "TotalSquareFootage", c => c.Int());
            AddColumn("dbo.Assets", "SgiPercent", c => c.Single());
            DropColumn("dbo.Assets", "TotalSqareFootage1");
            DropColumn("dbo.Assets", "SgiPercet");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "SgiPercet", c => c.Single());
            AddColumn("dbo.Assets", "TotalSqareFootage1", c => c.Int());
            DropColumn("dbo.Assets", "SgiPercent");
            DropColumn("dbo.Assets", "TotalSquareFootage");
        }
    }
}
