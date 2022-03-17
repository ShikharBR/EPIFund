namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fee_Simple_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "RenovatedByOwner", c => c.Boolean());
            AddColumn("dbo.Assets", "RenovationYear", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "RenovationBudget", c => c.Double());
            AddColumn("dbo.Assets", "IndicativeBidsDueDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "IndicativeBidsDueDate");
            DropColumn("dbo.Assets", "RenovationBudget");
            DropColumn("dbo.Assets", "RenovationYear");
            DropColumn("dbo.Assets", "RenovatedByOwner");
        }
    }
}
