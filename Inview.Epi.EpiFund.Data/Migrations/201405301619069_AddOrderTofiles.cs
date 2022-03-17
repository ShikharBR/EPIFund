namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderTofiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetDocuments", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.AssetImages", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssetImages", "Order");
            DropColumn("dbo.AssetDocuments", "Order");
        }
    }
}
